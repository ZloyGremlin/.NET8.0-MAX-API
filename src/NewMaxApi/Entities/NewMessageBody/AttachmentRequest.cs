using NewMaxApi.Enums;
using NewMaxApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities
{
    [JsonConverter(typeof(AttachmentRequestConverter))]
    public abstract class AttachmentRequest
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = string.Empty;
    }

    public class AttachmentRequestImage : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public PhotoAttachmentRequestPayload Payload { get; set; } = new();
    }

    public class AttachmentRequestVideo : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public UploadedInfo Payload { get; set; } = new();
    }

    public class AttachmentRequestAudio : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public UploadedInfo Payload { get; set; } = new();
    }

    public class AttachmentRequestFile : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public UploadedInfo Payload { get; set; } = new();
    }

    public class AttachmentRequestSticker : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public StickerAttachmentRequestPayload Payload { get; set; } = new();
    }

    public class AttachmentRequestContact : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public ContactAttachmentRequestPayload Payload { get; set; } = new();
    }

    public class AttachmentRequestInlineKeyboard : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public InlineKeyboardAttachmentRequestPayload Payload { get; set; } = new();
    }

    public class AttachmentRequestLocation : AttachmentRequest
    {
        [JsonPropertyName("latitude")]
        public Double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public Double Longitude { get; set; }
    }

    public class AttachmentRequestShare : AttachmentRequest
    {
        [JsonPropertyName("payload")]
        public ShareAttachmentPayload Payload { get; set; } = new();
    }
}
