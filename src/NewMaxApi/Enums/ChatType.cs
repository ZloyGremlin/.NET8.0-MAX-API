using System.Text.Json.Serialization;

namespace NewMaxApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<ChatType>))]
    public enum ChatType
    {
        [JsonPropertyName("dialog")]
        Dialog,

        [JsonPropertyName("chat")]
        Chat
    }
}
