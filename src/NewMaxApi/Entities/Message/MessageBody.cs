using System.Text.Json.Serialization;
using NewMaxApi.Entities;

namespace NewMaxApi.Entities
{
    public class MessageBody
    {
        [JsonPropertyName("mid")]
        public string Mid { get; set; }

        [JsonPropertyName("seq")]
        public long Seq { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("attachments")]
        public Attachment[]? Attachments { get; set; }

        [JsonPropertyName("markup")]
        public MarkupElement[]? Markup { get; set; }
    }
}
