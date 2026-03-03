using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class Subscription
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("update_types")]
        public string[]? UpdateTypes { get; set; }
    }
}
