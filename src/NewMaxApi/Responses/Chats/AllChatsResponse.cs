using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Chats
{
    public class AllChatsResponse
    {
        [JsonPropertyName("chats")]
        public Chat[] Chats { get; set; }

        [JsonPropertyName("marker")]
        public long? Marker { get; set; }
    }
}
