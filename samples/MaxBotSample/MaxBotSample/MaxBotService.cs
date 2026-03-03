using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewMaxApi;
using NewMaxApi.Entities;
using NewMaxApi.Requests.Messages;

namespace TestBot;

public sealed class MaxBotService : IHostedService, IDisposable
{
    private readonly ILogger<MaxBotService> _logger;
    private readonly MaxApiProvider _max;

    private CancellationTokenSource? _cts;
    private Task? _pollingTask;

    private long? _lastUpdateMarker;
    private int _consecutiveErrors;
    private DateTimeOffset _lastHeartbeatLogUtc = DateTimeOffset.MinValue;

    // Настройки long polling / retry
    private const int PollLimit = 100;
    private const int PollTimeoutSeconds = 30; // можно 90, если хочешь длиннее long-poll
    private static readonly TimeSpan HeartbeatInterval = TimeSpan.FromMinutes(1);

    public MaxBotService(MaxApiProvider maxApiProvider, ILogger<MaxBotService> logger)
    {
        _max = maxApiProvider ?? throw new ArgumentNullException(nameof(maxApiProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_cts != null)
            throw new InvalidOperationException("Service already started.");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        _pollingTask = RunPollingLoopAsync(_cts.Token);

        // Чтобы не было "тихого" падения фоновой задачи без логов
        _ = _pollingTask.ContinueWith(t =>
        {
            if (t.IsFaulted && t.Exception != null)
            {
                _logger.LogError(t.Exception, "Polling task crashed unexpectedly.");
            }
            else if (t.IsCanceled)
            {
                _logger.LogInformation("Polling task canceled.");
            }
            else
            {
                _logger.LogWarning("Polling task completed unexpectedly.");
            }
        }, TaskScheduler.Default);

        _logger.LogInformation("MAX bot started.");
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_cts == null)
            return;

        _logger.LogInformation("MAX bot stopping...");

        try
        {
            _cts.Cancel();
        }
        catch
        {
            // ignore
        }

        if (_pollingTask != null)
        {
            try
            {
                await _pollingTask.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Ожидаемо при остановке
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while stopping polling task.");
            }
        }

        _logger.LogInformation("MAX bot stopped.");
    }

    public void Dispose()
    {
        try
        {
            _cts?.Cancel();
        }
        catch
        {
            // ignore
        }

        _cts?.Dispose();
    }

    // -----------------------------
    // Polling
    // -----------------------------
    private async Task RunPollingLoopAsync(CancellationToken ct)
    {
        _logger.LogInformation(
            "Polling loop started. timeout={TimeoutSec}s, limit={Limit}",
            PollTimeoutSeconds, PollLimit);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                LogHeartbeatIfNeeded();

                var resp = await _max.Subscriptions.GetUpdatesAsync(
                    limit: PollLimit,
                    timeout: PollTimeoutSeconds,
                    marker: _lastUpdateMarker,
                    cancellationToken: ct);

                // Успешный запрос — сбрасываем счетчик ошибок
                _consecutiveErrors = 0;

                var updates = resp?.Updates;
                var count = updates?.Length ?? 0;

                _logger.LogDebug("Polling response received. updates={Count}, marker={Marker}", count, _lastUpdateMarker);

                if (count == 0)
                    continue;

                _logger.LogInformation("Received {Count} update(s)", count);

                foreach (var u in updates!)
                {
                    try
                    {
                        await ProcessUpdateAsync(u, ct);

                        // Best-effort marker update. Если в API есть resp.Marker — лучше брать его.
                        _lastUpdateMarker = Math.Max(_lastUpdateMarker ?? 0, u.Timestamp);
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        // Один апдейт не должен валить весь цикл
                        _logger.LogError(ex,
                            "Failed to process update. type={Type}, ts={Ts}",
                            u.UpdateType, u.Timestamp);
                    }
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogInformation("Polling loop canceled.");
                break;
            }
            catch (TaskCanceledException ex)
            {
                // Часто сюда попадают сетевые timeout/cancel на запросе
                _consecutiveErrors++;
                var delay = GetBackoffDelay(_consecutiveErrors);

                _logger.LogWarning(ex,
                    "Polling request canceled/timed out unexpectedly. consecutiveErrors={Errors}, retryIn={DelayMs}ms",
                    _consecutiveErrors, (int)delay.TotalMilliseconds);

                await DelaySafeAsync(delay, ct);
            }
            catch (Exception ex)
            {
                _consecutiveErrors++;
                var delay = GetBackoffDelay(_consecutiveErrors);

                _logger.LogError(ex,
                    "Polling loop error. consecutiveErrors={Errors}, retryIn={DelayMs}ms",
                    _consecutiveErrors, (int)delay.TotalMilliseconds);

                await DelaySafeAsync(delay, ct);
            }
        }

        _logger.LogInformation("Polling loop finished.");
    }

    private void LogHeartbeatIfNeeded()
    {
        var now = DateTimeOffset.UtcNow;
        if (now - _lastHeartbeatLogUtc >= HeartbeatInterval)
        {
            _lastHeartbeatLogUtc = now;
            _logger.LogInformation(
                "Polling heartbeat: alive, marker={Marker}, consecutiveErrors={Errors}, utc={UtcTime}",
                _lastUpdateMarker, _consecutiveErrors, now);
        }
    }

    private static TimeSpan GetBackoffDelay(int consecutiveErrors)
    {
        // 2s, 5s, 10s, 15s, 30s (cap)
        return consecutiveErrors switch
        {
            <= 1 => TimeSpan.FromSeconds(2),
            2 => TimeSpan.FromSeconds(5),
            3 => TimeSpan.FromSeconds(10),
            4 => TimeSpan.FromSeconds(15),
            _ => TimeSpan.FromSeconds(30)
        };
    }

    private static async Task DelaySafeAsync(TimeSpan delay, CancellationToken ct)
    {
        try
        {
            await Task.Delay(delay, ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // норм при остановке
        }
    }

    // -----------------------------
    // Dispatch: handlers for ALL known update types
    // -----------------------------
    private async Task ProcessUpdateAsync(Update update, CancellationToken ct)
    {
        _logger.LogInformation("Update received: type={Type}, ts={Ts}", update.UpdateType, update.Timestamp);

        switch (update)
        {
            // ===== Messages =====
            case MessageCreated u:
                await OnMessageCreated(u, ct);
                return;

            case MessageCallback u:
                await OnMessageCallback(u, ct);
                return;

            case MessageEdited u:
                OnMessageEdited(u);
                return;

            case MessageRemoved u:
                OnMessageRemoved(u);
                return;

            // ===== Bot lifecycle =====
            case BotStarted u:
                await OnBotStarted(u, ct);
                return;

            case BotAdded u:
                OnBotAdded(u);
                return;

            case BotRemoved u:
                OnBotRemoved(u);
                return;

            case BotStopped u:
                OnBotStopped(u);
                return;

            // ===== Dialog events =====
            case DialogMuted u:
                OnDialogMuted(u);
                return;

            case DialogUnmuted u:
                OnDialogUnmuted(u);
                return;

            case DialogCleared u:
                OnDialogCleared(u);
                return;

            case DialogRemoved u:
                OnDialogRemoved(u);
                return;

            // ===== User membership =====
            case UserAdded u:
                OnUserAdded(u);
                return;

            case UserRemoved u:
                OnUserRemoved(u);
                return;

            // ===== Chat meta =====
            case ChatTitleChanged u:
                OnChatTitleChanged(u);
                return;

            case MessageChatCreated u:
                OnMessageChatCreated(u);
                return;

            default:
                _logger.LogWarning("Unhandled update type: {Type}", update.UpdateType);
                return;
        }
    }

    // -----------------------------
    // Minimal per-type handlers
    // -----------------------------
    private async Task OnBotStarted(BotStarted u, CancellationToken ct)
    {
        var chatId = u.ChatId;
        _logger.LogInformation("BotStarted chat={ChatId} payload='{Payload}'", chatId, u.Payload);

        await SafeSendTextAsync(
            chatId,
            "✅ BotStarted получен. Я на связи.\nНапиши что-нибудь или нажми кнопку.",
            ct,
            attachments: Keyboards.Ping());
    }

    private async Task OnMessageCreated(MessageCreated u, CancellationToken ct)
    {
        var chatId = u.Message.Recipient.ChatId;
        var text = u.Message.Body?.Text?.Trim();
        var attachmentsCount = u.Message.Body?.Attachments?.Length ?? 0;

        _logger.LogInformation(
            "MessageCreated chat={ChatId} text='{Text}' attachments={AttachmentsCount}",
            chatId, text, attachmentsCount);

        // Минимальный ответ: подтверждаем прием
        if (!string.IsNullOrWhiteSpace(text))
        {
            if (text.Equals("/start", StringComparison.OrdinalIgnoreCase) ||
                text.Equals("ping", StringComparison.OrdinalIgnoreCase))
            {
                await SafeSendTextAsync(chatId, "pong ✅", ct, Keyboards.Ping());
                return;
            }

            await SafeSendTextAsync(chatId, $"✅ Получил текст: {text}", ct, Keyboards.Ping());
            return;
        }

        if (attachmentsCount > 0)
        {
            await SafeSendTextAsync(
                chatId,
                $"✅ Получил вложение(я): {attachmentsCount}. (В тестовом режиме не обрабатываю)",
                ct,
                Keyboards.Ping());
            return;
        }

        await SafeSendTextAsync(chatId, "✅ Сообщение без текста/вложений получено.", ct, Keyboards.Ping());
    }

    private async Task OnMessageCallback(MessageCallback u, CancellationToken ct)
    {
        var chatId = u.Message.Recipient.ChatId;
        var payload = u.Callback.Payload?.Trim() ?? string.Empty;

        _logger.LogInformation("MessageCallback chat={ChatId} payload='{Payload}'", chatId, payload);

        if (payload.Equals("ping", StringComparison.OrdinalIgnoreCase))
        {
            await SafeSendTextAsync(chatId, "pong ✅ (callback)", ct, Keyboards.Ping());
            return;
        }

        await SafeSendTextAsync(chatId, $"✅ Callback получен: {payload}", ct, Keyboards.Ping());
    }

    private void OnMessageEdited(MessageEdited u)
    {
        var chatId = u.Message.Recipient.ChatId;
        _logger.LogInformation("MessageEdited chat={ChatId}", chatId);
    }

    private void OnMessageRemoved(MessageRemoved u)
    {
        _logger.LogInformation("MessageRemoved chat={ChatId}", u.ChatId);
    }

    private void OnBotAdded(BotAdded u)
    {
        _logger.LogInformation("BotAdded chat={ChatId}", u.ChatId);
    }

    private void OnBotRemoved(BotRemoved u)
    {
        _logger.LogInformation("BotRemoved chat={ChatId}", u.ChatId);
    }

    private void OnBotStopped(BotStopped u)
    {
        _logger.LogInformation("BotStopped chat={ChatId}", u.ChatId);
    }

    private void OnDialogMuted(DialogMuted u)
    {
        _logger.LogInformation("DialogMuted chat={ChatId}", u.ChatId);
    }

    private void OnDialogUnmuted(DialogUnmuted u)
    {
        _logger.LogInformation("DialogUnmuted chat={ChatId}", u.ChatId);
    }

    private void OnDialogCleared(DialogCleared u)
    {
        _logger.LogInformation("DialogCleared chat={ChatId}", u.ChatId);
    }

    private void OnDialogRemoved(DialogRemoved u)
    {
        _logger.LogInformation("DialogRemoved chat={ChatId}", u.ChatId);
    }

    private void OnUserAdded(UserAdded u)
    {
        _logger.LogInformation("UserAdded chat={ChatId} user={UserId}", u.ChatId, u.User.UserId);
    }

    private void OnUserRemoved(UserRemoved u)
    {
        _logger.LogInformation("UserRemoved chat={ChatId} user={UserId}", u.ChatId, u.User.UserId);
    }

    private void OnChatTitleChanged(ChatTitleChanged u)
    {
        _logger.LogInformation("ChatTitleChanged chat={ChatId} title='{Title}'", u.ChatId, u.Title);
    }

    private void OnMessageChatCreated(MessageChatCreated u)
    {
        _logger.LogInformation("MessageChatCreated chat={ChatId}", u.Chat.ChatId);
    }

    // -----------------------------
    // Send helpers
    // -----------------------------
    private async Task SafeSendTextAsync(long chatId, string text, CancellationToken ct, AttachmentRequest[]? attachments = null)
    {
        try
        {
            await SendTextAsync(chatId, text, ct, attachments);
            _logger.LogInformation("Reply sent to chat={ChatId}", chatId);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message to chat={ChatId}", chatId);
        }
    }

    private async Task SendTextAsync(long chatId, string text, CancellationToken ct, AttachmentRequest[]? attachments = null)
    {
        var req = new SendMessageRequest
        {
            Text = text,
            Attachments = attachments
        };

        await _max.Messages.SendMessageAsync(body: req, chatId: chatId, cancellationToken: ct);
    }

    // -----------------------------
    // Minimal keyboard factory
    // -----------------------------
    private static class Keyboards
    {
        public static AttachmentRequest[] Ping()
            => Inline(new[]
            {
                Btn("ping", "ping")
            });

        private static AttachmentRequest[] Inline(Button[] buttons)
        {
            var grid = buttons.Select(b => new System.Collections.Generic.List<Button> { b }).ToList();
            var payload = new InlineKeyboardAttachmentRequestPayload { Buttons = grid };
            var inline = new AttachmentRequestInlineKeyboard
            {
                Type = "inline_keyboard",
                Payload = payload
            };
            return new AttachmentRequest[] { inline };
        }

        private static Button Btn(string text, string payload)
            => new ButtonCallback
            {
                ButtonType = "callback",
                ButtonText = text,
                Payload = payload
            };
    }
}