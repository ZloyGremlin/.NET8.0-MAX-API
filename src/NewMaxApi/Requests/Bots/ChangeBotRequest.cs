using NewMaxApi.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Requests.Bots
{
    public class ChangeBotRequest
    {
        [JsonPropertyName("first_name")]
        [MinLength(1)]
        [MaxLength(64)]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [MinLength(1)]
        [MaxLength(64)]
        public string? LastName { get; set; }

        [JsonPropertyName("description")]
        [MinLength(1)]
        [MaxLength(16_000)]
        public string? Description { get; set; }

        [JsonPropertyName("commands")]
        [MaxLength(32)]
        public BotCommand[]? BotCommands { get; set; }

        [JsonPropertyName("photo")]
        public PhotoAttachmentRequestPayload? Photo { get; set; }
    }
}
