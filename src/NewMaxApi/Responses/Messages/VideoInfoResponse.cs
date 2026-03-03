using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Messages
{
    public class VideoInfoResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("urls")]
        public VideoUrls? Urls { get; set; }

        [JsonPropertyName("thumbnail")]
        public PhotoAttachmentPayload? Thumbnail { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }
    }
}
