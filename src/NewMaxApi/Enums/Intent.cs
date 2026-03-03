using System.Text.Json.Serialization;

namespace NewMaxApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<Intent>))]
    public enum Intent
    {
        [JsonPropertyName("default")]
        Default,

        [JsonPropertyName("positive")]
        Positive,

        [JsonPropertyName("negative")]
        Negative
    }
}
