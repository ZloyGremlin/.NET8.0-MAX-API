using System.Text.Json.Serialization;

namespace NewMaxApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<ChatStatus>))]
    public enum ChatStatus
    {
        [JsonPropertyName("active")]
        Active,

        [JsonPropertyName("removed")]
        Removed,

        [JsonPropertyName("left")]
        Left,

        [JsonPropertyName("closed")]
        Closed,

        [JsonPropertyName("suspended")]
        Suspended
    }
}
