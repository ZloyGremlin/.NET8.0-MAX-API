using NewMaxApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities
{
    public class Payload
    {
    }

    public class MediaAttachmentPayload : Payload
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    public class PhotoAttachmentPayload : Payload
    {
        [JsonPropertyName("photo_id")]
        public long PhotoId { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class FileAttachmentPayload : Payload
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    public class StickerAttachmentPayload : Payload
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class ContactAttachmentPayload : Payload
    {
        [JsonPropertyName("vcf_info")]
        public string? VcfInfo { get; set; }

        [JsonPropertyName("max_info")]
        public User? MaxInfo { get; set; }
    }

    public class Keyboard : Payload
    {
        [JsonPropertyName("buttons")]
        public List<List<Button>> Buttons { get; set; } = new();
    }

    public class ShareAttachmentPayload : Payload
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}
