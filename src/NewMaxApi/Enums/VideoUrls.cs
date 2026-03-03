using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class VideoUrls
    {
        [JsonPropertyName("mp4_1080")]
        public string? Mp4_1080 { get; set; }

        [JsonPropertyName("mp4_720")]
        public string? Mp4_720 { get; set; }

        [JsonPropertyName("mp4_480")]
        public string? Mp4_480 { get; set; }

        [JsonPropertyName("mp4_360")]
        public string? Mp4_360 { get; set; }

        [JsonPropertyName("mp4_240")]
        public string? Mp4_240 { get; set; }

        [JsonPropertyName("mp4_144")]
        public string? Mp4_144 { get; set; }

        [JsonPropertyName("hls")]
        public string? Hls { get; set; }
    }
}
