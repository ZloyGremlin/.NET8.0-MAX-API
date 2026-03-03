using System.Text.Json.Serialization;

namespace NewMaxApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<MessageLinkType>))]
    public enum MessageLinkType
    {
        [JsonPropertyName("forward")]
        Forward,

        [JsonPropertyName("reply")]
        Reply
    }
}
