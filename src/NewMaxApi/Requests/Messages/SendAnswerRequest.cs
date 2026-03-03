using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Requests.Messages
{
    public class SendAnswerRequest
    {
        [JsonPropertyName("message")]
        public NewMessageBodyClass? Message { get; set; }

        [JsonPropertyName("notification")]
        public string? Notification { get; set; }
    }
}
