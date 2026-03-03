using NewMaxApi.Providers;
using NewMaxApi.Requests.Messages;
using NewMaxApi.Resources;
using NewMaxApi.Responses;
using NewMaxApi.Responses.Messages;
using System.Linq;
using System.Net.Http;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace NewMaxApi
{
    /// <summary>
    /// Предоставляет методы для взаимодействия с конечными точками, связанными с сообщениями, в API MAX.
    /// </summary>
    /// <param name="accessToken">Токен доступа, используемый для аутентификации API.</param>
    /// <param name="httpClient">Экземпляр <see cref="HttpClient"/>, используемый для запросов API.</param>
    public partial class MessagesProvider(string accessToken, HttpClient httpClient) : BaseProvider(httpClient)
    {
        protected override string PathTemplate => "/messages";

        /// <summary>
        /// Возвращает сообщения чата: страницу с результатами и маркером, указывающим на следующую страницу.
        /// </summary>
        /// <remarks>
        /// Сообщения возвращаются в обратном порядке, то есть последние сообщения чата будут первыми в массиве. Поэтому, если вы используете параметры <paramref name="from"/> и <paramref name="to"/>, то <paramref name="to"/> должно быть меньше <paramref name="from"/>.
        /// </remarks>
        /// <param name="chatId">Идентификатор чата для получения сообщений из определенного чата.</param>
        /// <param name="messageIds">Список идентификаторов сообщений для получения.</param>
        /// <param name="from">Время начала для запрошенных сообщений (в формате временной метки Unix).</param>
        /// <param name="to">Время окончания для запрошенных сообщений (в формате временной метки Unix).</param>
        /// <param name="count">Максимальное количество сообщений в ответе.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<MessagesResponse> GetMessagesAsync(long chatId, string[]? messageIds = null, long? from = null, long? to = null, int? count = null, CancellationToken cancellationToken = default)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chatId);

            if (from != null && to != null && from > to)
                throw new ArgumentException(string.Format(Error.FromToError, to, from));

            var requestUri = new StringBuilder($"{PathTemplate}?chat_id={chatId}");

            if (messageIds?.Length > 0)
                requestUri.Append($"&message_ids={string.Join(',', messageIds)}");

            if (from != null && from > 0)
                requestUri.Append($"&from={from}");

            if (to != null && to > 0)
                requestUri.Append($"&to={to}");

            if (count != null && count >= 1 && count <= 100)
                requestUri.Append($"&count={count}");

            requestUri.Append($"&access_token={accessToken}");

            return await HttpClientMax.GetAsync<MessagesResponse>(requestUri.ToString(), cancellationToken);
        }

        /// <summary>
        /// Отправляет сообщение в чат.
        /// </summary>
        /// <param name="body">Тело запроса.</param>
        /// <param name="userId">Если вы хотите отправить сообщение пользователю, укажите его ID.</param>
        /// <param name="chatId">Если сообщение отправляется в чат, укажите его ID.</param>
        /// <param name="disableLinkPreview">Если <c>false</c>, сервер не будет генерировать предварительный просмотр ссылок в тексте сообщения.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SpecMessageResponse> SendMessageAsync(SendMessageRequest body, long? userId = null, long? chatId = null, bool? disableLinkPreview = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(body);

            var requestUri = new StringBuilder($"{PathTemplate}?access_token={accessToken}");

            if (userId != null && userId > 0)
                requestUri.Append($"&user_id={userId}");

            if (chatId != null && chatId > 0)
                requestUri.Append($"&chat_id={chatId}");

            if (disableLinkPreview != null)
                requestUri.Append($"&disable_link_preview={disableLinkPreview}");

            return await HttpClientMax.PostAsync<SpecMessageResponse>(requestUri.ToString(), body, cancellationToken);
        }

        /// <summary>
        /// Редактирует сообщение в чате.
        /// </summary>
        /// <remarks>
        /// Если поле вложений равно <c>null</c>, вложения текущего сообщения не изменяются. Если в этом поле передан пустой список, все вложения будут удалены.
        /// </remarks>
        /// <param name="messageId">Идентификатор редактируемого сообщения.</param>
        /// <param name="body">Тело запроса.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> EditMessageAsync(string messageId, SendMessageRequest body, CancellationToken cancellationToken = default)
        {
            if (!MessageRegex().IsMatch(messageId))
                throw new ArgumentException(Error.MessageRegexError, nameof(messageId));

            ArgumentNullException.ThrowIfNull(body);

            return await HttpClientMax.PutAsync<SuccessResponse>($"{PathTemplate}?access_token={accessToken}", body, cancellationToken);
        }

        /// <summary>
        /// Удаляет сообщение в диалоге или чате, если у бота есть разрешение на удаление сообщений.
        /// </summary>
        /// <param name="messageId">Идентификатор удаляемого сообщения.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> DeleteMessageAsync(string messageId, CancellationToken cancellationToken = default)
        {
            if (!MessageRegex().IsMatch(messageId))
                throw new ArgumentException(Error.MessageRegexError, nameof(messageId));

            return await HttpClientMax.DeleteAsync<SuccessResponse>($"{PathTemplate}?message_id={messageId}&access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Возвращает одно сообщение по его идентификатору.
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения (mid) для получения одного сообщения в чате.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<GetMessageResponse> GetMessageAsync(string messageId, CancellationToken cancellationToken = default)
        {
            if (!MessageRegex().IsMatch(messageId))
                throw new ArgumentException(Error.MessageRegexError, nameof(messageId));

            return await HttpClientMax.GetAsync<GetMessageResponse>($"{PathTemplate}/{messageId}?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Возвращает подробную информацию о прикреплённом видео. URL-адреса воспроизведения и дополнительные метаданные.
        /// </summary>
        /// <param name="videoToken">Токен прикрепленного видео.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<VideoInfoResponse> GetVideoInfoAsync(string videoToken, CancellationToken cancellationToken = default)
        {
            if (!MessageRegex().IsMatch(videoToken))
                throw new ArgumentException(Error.MessageRegexError, nameof(videoToken));

            return await HttpClientMax.GetAsync<VideoInfoResponse>($"/videos/{videoToken}", cancellationToken);
        }

        /// <summary>
        /// Этот метод используется для отправки ответа после нажатия пользователем кнопки. Ответ может представлять собой обновленное сообщение и/или однократное уведомление пользователю.
        /// </summary>
        /// <param name="callbackId">Идентификатор кнопки, которую нажал пользователь. Бот получает этот идентификатор в рамках обновления с типом message_callback.</param>
        /// <param name="body">Тело запроса.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> SendAnswerAsync(string callbackId, SendAnswerRequest body, CancellationToken cancellationToken = default)
        {
            if (!CallbackRegex().IsMatch(callbackId))
                throw new ArgumentException(Error.CallbackRegexError, nameof(callbackId));

            ArgumentNullException.ThrowIfNull(body);

            return await HttpClientMax.PostAsync<SuccessResponse>($"/answers?access_token={accessToken}", body, cancellationToken);
        }

        [GeneratedRegex("[a-zA-Z0-9_\\-]+")]
        private static partial Regex MessageRegex();

        [GeneratedRegex("^(?!\\s*$).+")]
        private static partial Regex CallbackRegex();
    }
}
