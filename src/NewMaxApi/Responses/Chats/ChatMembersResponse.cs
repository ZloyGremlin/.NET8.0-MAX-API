using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Chats
{
    public class ChatMembersResponse
    {
        [JsonPropertyName("members")]
        public ChatMembershipResponse[] Members { get; set; }

        [JsonPropertyName("marker")]
        public long? Marker { get; set; }
    }
}
