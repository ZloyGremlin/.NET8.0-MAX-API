using NewMaxApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities
{
    public class AttachmentRequestPayload
    {
    }

    public class PhotoAttachmentRequestPayload : AttachmentRequestPayload
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("photos")]
        public object? Photos { get; set; }
    }

    public class UploadedInfo : AttachmentRequestPayload
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    public class StickerAttachmentRequestPayload : AttachmentRequestPayload
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class ContactAttachmentRequestPayload : AttachmentRequestPayload
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("contact_id")]
        public long? ContactId { get; set; }

        [JsonPropertyName("vcf_info")]
        public string? VcfInfo { get; set; }

        [JsonPropertyName("vcf_phone")]
        public string? VcfPhone { get; set; }
    }

    public class InlineKeyboardAttachmentRequestPayload : AttachmentRequestPayload
    {
        [JsonPropertyName("buttons")]
        public List<List<Button>> Buttons { get; set; } = new();
    }
}
