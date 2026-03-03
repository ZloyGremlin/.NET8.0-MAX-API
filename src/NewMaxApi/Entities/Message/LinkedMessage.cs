using NewMaxApi.Enums;
using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class LinkedMessage
    {
        [JsonPropertyName("type")]
        public MessageLinkType Type { get; set; }

        [JsonPropertyName("sender")]
        public User Sender { get; set; }

        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("message")]
        public MessageBody Message { get; set; }
    }
}
