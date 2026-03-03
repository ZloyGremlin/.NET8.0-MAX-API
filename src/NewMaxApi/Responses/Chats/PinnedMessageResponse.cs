using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Chats
{
    public class PinnedMessageResponse
    {
        [JsonPropertyName("message")]
        public MessageClass? Message { get; set; }
    }
}
