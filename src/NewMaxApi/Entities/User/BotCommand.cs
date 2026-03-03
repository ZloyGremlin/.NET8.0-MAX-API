using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class BotCommand
    {
        [JsonPropertyName("name")]
        [MinLength(1)]
        [MaxLength(64)]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        [MinLength(1)]
        [MaxLength(128)]
        public string? Description { get; set; }
    }
}
