using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Uploads
{
    public class UploadResponse
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    public class UploadFileResponse
    {
        [JsonPropertyName("fileId")]
        public long FileId { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    public class UploadImageResponse
    {
        [JsonPropertyName("photos")]
        public object? Photos { get; set; }
    }


}
