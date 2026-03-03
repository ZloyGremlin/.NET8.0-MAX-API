using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class MessageClass
    {
        [JsonPropertyName("sender")]
        public User Sender { get; set; }

        [JsonPropertyName("recipient")]
        public Recipient Recipient { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("link")]
        public LinkedMessage? Link { get; set; }

        [JsonPropertyName("body")]
        public MessageBody Body { get; set; }

        [JsonPropertyName("stat")]
        public MessageStat? Stat { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
