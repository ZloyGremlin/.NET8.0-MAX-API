using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class UserWithPhoto : User
    {
        [JsonPropertyName("description")]
        [MaxLength(16_000)]
        public string? Description { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("full_avatar_url")]
        public string FullAvatarUrl { get; set; }
    }
}
