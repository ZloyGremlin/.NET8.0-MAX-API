using System.Text.Json.Serialization;

namespace NewMaxApi.Requests.Subscriptions
{
    public class SubscribeRequest
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("update_types")]
        public string[] UpdateTypes { get; set; }

        [JsonPropertyName("secret")]
        public string Secret { get; set; }
    }
}
