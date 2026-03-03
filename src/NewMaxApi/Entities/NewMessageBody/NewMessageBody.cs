using NewMaxApi.Entities;
using NewMaxApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class NewMessageBodyClass
    {
        [JsonPropertyName("text")]
        [MaxLength(4000)]
        public string? Text { get; set; }

        [JsonPropertyName("attachments")]
        public AttachmentRequest[]? Attachments { get; set; }

        [JsonPropertyName("link")]
        public NewMessageLink? Link { get; set; }

        [JsonPropertyName("notify")]
        public bool Notify { get; set; }

        [JsonPropertyName("format")]
        public TextFormat? Format { get; set; }
    }
}
