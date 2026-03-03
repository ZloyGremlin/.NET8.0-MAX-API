using System.Text.Json.Serialization;

namespace NewMaxApi.Requests.Chats
{
    public class PinMessageRequest
    {
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }

        [JsonPropertyName("notify")]
        public bool? Notify { get; set; }
    }
}
