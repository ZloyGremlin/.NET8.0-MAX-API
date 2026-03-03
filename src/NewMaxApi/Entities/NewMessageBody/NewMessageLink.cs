using NewMaxApi.Enums;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class NewMessageLink
    {
        [JsonPropertyName("type")]
        public MessageLinkType Type { get; set; }

        [JsonPropertyName("mid")]
        public string Mid { get; set; }
    }
}
