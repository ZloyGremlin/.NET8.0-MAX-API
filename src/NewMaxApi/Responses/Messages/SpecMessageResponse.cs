using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Messages
{
    public class SpecMessageResponse
    {
        [JsonPropertyName("message")]
        public MessageClass Message { get; set; }
    }
}
