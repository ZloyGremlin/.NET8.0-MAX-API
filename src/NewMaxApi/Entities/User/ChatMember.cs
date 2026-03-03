using NewMaxApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class ChatMember : User
    {
        [JsonPropertyName("description")]
        [MaxLength(16_000)]
        public string? Description { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("full_avatar_url")]
        public string FullAvatarUrl { get; set; }

        [JsonPropertyName("last_access_time")]
        public long LastAccessTime { get; set; }

        [JsonPropertyName("is_owner")]
        public bool IsOwner { get; set; }

        [JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }

        [JsonPropertyName("join_time")]
        public long JoinTime { get; set; }

        [JsonPropertyName("permissions")]
        public ChatPermission? Permissions { get; set; }

        [JsonPropertyName("alias")]
        public string Alias { get; set; }
    }
}
