using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter<UploadType>))]
    public enum UploadType
    {
        [JsonPropertyName("image")]
        Image,

        [JsonPropertyName("video")]
        Video,

        [JsonPropertyName("audio")]
        Audio,

        [JsonPropertyName("file")]
        File
    }
}
