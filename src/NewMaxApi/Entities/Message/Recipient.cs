using NewMaxApi.Enums;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class Recipient
    {
        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("chat_type")]
        public ChatType ChatType { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }
    }
}
