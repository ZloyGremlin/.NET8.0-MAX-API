using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Messages
{
    public class MessagesResponse
    {
        [JsonPropertyName("messages")]
        public MessageClass[] Messages { get; set; }
    }
}
