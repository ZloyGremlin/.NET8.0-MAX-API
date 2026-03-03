using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    [JsonConverter(typeof(UpdateConverter))]
    public abstract class Update
    {
        [JsonPropertyName("update_type")]
        public string UpdateType { get; init; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
    public class MessageCreated : Update
    {
        public MessageCreated() => UpdateType = "message_created";

        [JsonPropertyName("message")]
        public MessageClass Message { get; set; } = new();

        [JsonPropertyName("user_locale")]
        public string? UserLocale { get; set; }
    }

    public class MessageCallback : Update
    {
        public MessageCallback() => UpdateType = "message_callback";

        [JsonPropertyName("callback")]
        public Callback Callback { get; set; } = new();

        [JsonPropertyName("message")]
        public MessageClass Message { get; set; } = new();

        [JsonPropertyName("user_locale")]
        public string? UserLocale { get; set; }
    }

    public class MessageEdited : Update
    {
        public MessageEdited() => UpdateType = "message_edited";

        [JsonPropertyName("message")]
        public MessageClass Message { get; set; } = new();
    }

    public class MessageRemoved : Update
    {
        public MessageRemoved() => UpdateType = "message_removed";

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; } = string.Empty;

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }
    }

    public class BotAdded : Update
    {
        public BotAdded() => UpdateType = "bot_added";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("is_channel")]
        public bool IsChannel { get; set; }
    }

    public class BotRemoved : Update
    {
        public BotRemoved() => UpdateType = "bot_removed";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("is_channel")]
        public bool IsChannel { get; set; }
    }

    public class DialogMuted : Update
    {
        public DialogMuted() => UpdateType = "dialog_muted";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("muted_until")]
        public long MutedUntil { get; set; }

        [JsonPropertyName("user_locale")]
        public string UserLocale { get; set; } = string.Empty;
    }

    public class DialogUnmuted : Update
    {
        public DialogUnmuted() => UpdateType = "dialog_unmuted";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("user_locale")]
        public string UserLocale { get; set; } = string.Empty;
    }

    public class DialogCleared : Update
    {
        public DialogCleared() => UpdateType = "dialog_cleared";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("user_locale")]
        public string UserLocale { get; set; } = string.Empty;
    }

    public class DialogRemoved : Update
    {
        public DialogRemoved() => UpdateType = "dialog_removed";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("user_locale")]
        public string UserLocale { get; set; } = string.Empty;
    }

    public class UserAdded : Update
    {
        public UserAdded() => UpdateType = "user_added";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("inviter_id")]
        public long? InviterId { get; set; }

        [JsonPropertyName("is_channel")]
        public bool IsChannel { get; set; }
    }

    public class UserRemoved : Update
    {
        public UserRemoved() => UpdateType = "user_removed";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("admin_id")]
        public long AdminId { get; set; }

        [JsonPropertyName("is_channel")]
        public bool IsChannel { get; set; }
    }

    public class BotStarted : Update
    {
        public BotStarted() => UpdateType = "bot_started";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("payload")]
        [MaxLength(512)]
        public string? Payload { get; set; }

        [JsonPropertyName("user_locale")]
        public string UserLocale { get; set; } = string.Empty;
    }

    public class BotStopped : Update
    {
        public BotStopped() => UpdateType = "bot_stopped";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("user_locale")]
        public string UserLocale { get; set; } = string.Empty;
    }

    public class ChatTitleChanged : Update
    {
        public ChatTitleChanged() => UpdateType = "chat_title_changed";

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = new();

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
    }

    public class MessageChatCreated : Update
    {
        public MessageChatCreated() => UpdateType = "message_chat_created";

        [JsonPropertyName("chat")]
        public Chat Chat { get; set; } = new();

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; } = string.Empty;

        [JsonPropertyName("start_payload")]
        public string? StartPayload { get; set; }
    }
}