using NewMaxApi.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Requests.Chats
{
    public class ChangeChatRequest
    {
        [JsonPropertyName("icon")]
        public PhotoAttachmentRequestPayload? Icon { get; set; }

        [JsonPropertyName("title")]
        [MinLength(1)]
        [MaxLength(200)]
        public string? Title { get; set; }

        [JsonPropertyName("pin")]
        public string? Pin { get; set; }

        [JsonPropertyName("notify")]
        public bool? Notify { get; set; }
    }
}
