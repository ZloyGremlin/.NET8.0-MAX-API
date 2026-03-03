using System.Text.Json.Serialization;

namespace NewMaxApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<TextFormat>))]
    public enum TextFormat
    {
        [JsonPropertyName("markdown")]
        Markdown,

        [JsonPropertyName("html")]
        Html
    }
}
