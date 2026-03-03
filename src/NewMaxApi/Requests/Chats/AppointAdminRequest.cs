using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Requests.Chats
{
    public class AppointAdminRequest
    {
        [JsonPropertyName("admins")]
        public ChatAdmin[] Admins { get; set; }

        [JsonPropertyName("marker")]
        public long? Marker { get; set; }
    }
}
