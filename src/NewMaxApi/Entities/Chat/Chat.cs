using NewMaxApi.Entities;
using NewMaxApi.Enums;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class Chat
    {
        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("type")]
        public ChatType Type { get; set; }

        [JsonPropertyName("status")]
        public ChatStatus Status { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("icon")]
        public Image? Icon { get; set; }

        [JsonPropertyName("last_event_time")]
        public long LastEventTime { get; set; }

        [JsonPropertyName("participants_count")]
        public int ParticipantsCount { get; set; }

        [JsonPropertyName("owner_id")]
        public long? OwnerId { get; set; }

        [JsonPropertyName("participants")]
        public object? Participants { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("dialog_with_user")]
        public UserWithPhoto? DialogWithUser { get; set; }

        [JsonPropertyName("chat_message_id")]
        public string? ChatMessageId { get; set; }

        [JsonPropertyName("pinned_message")]
        public MessageClass? PinnedMessage { get; set; }
    }
}
