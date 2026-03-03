using NewMaxApi.Enums;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class ChatAdmin
    {
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("permissions")]
        public ChatPermission[] Permissions { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }
    }
}
