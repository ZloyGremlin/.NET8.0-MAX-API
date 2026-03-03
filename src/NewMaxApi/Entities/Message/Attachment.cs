using NewMaxApi.Enums;
using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    [JsonConverter(typeof(AttachmentConverter))]
    public abstract class Attachment
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = string.Empty;
    }

    public class AttachmentImage : Attachment
    {
        [JsonPropertyName("payload")]
        public PhotoAttachmentPayload Payload {  get; set; } = new();
    }

    public class AttachmentVideo : Attachment
    {
        [JsonPropertyName("payload")]
        public MediaAttachmentPayload Payload { get; set; } = new();

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("width")]
        public long? Width { get; set; } 

        [JsonPropertyName("height")]
        public long? Height { get; set; }

        [JsonPropertyName("duration")]
        public long? Duration { get; set; }
    }

    public class AttachmentAudio : Attachment
    {
        [JsonPropertyName("payload")]
        public MediaAttachmentPayload Payload { get; set; } = new();

        [JsonPropertyName("transcription")]
        public string? Transcription { get; set; }
    }

    public class AttachmentFile : Attachment
    {
        [JsonPropertyName("payload")]
        public FileAttachmentPayload Payload { get; set; } = new();

        [JsonPropertyName("filename")]
        public string Filename { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public long Size { get; set; }
    }

    public class AttachmentStiker : Attachment
    {
        [JsonPropertyName("payload")]
        public StickerAttachmentPayload Payload { get; set; } = new();

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

    }

    public class AttachmentContact : Attachment
    {
        [JsonPropertyName("payload")]
        public ContactAttachmentPayload Payload { get; set; } = new();
    }

    public class AttachmentInlineKeyboard : Attachment
    {
        [JsonPropertyName("payload")]
        public Keyboard Payload { get; set; } = new();
    }

    public class AttachmentShare : Attachment
    {
        [JsonPropertyName("payload")]
        public ShareAttachmentPayload Payload { get; set; } = new();

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }
    }

    public class AttachmentLocation : Attachment
    {
        [JsonPropertyName("latitude")]
        public Double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public Double Longitude { get; set; }
    }
}
