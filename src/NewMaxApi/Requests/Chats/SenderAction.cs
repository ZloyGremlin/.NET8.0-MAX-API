using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Requests.Chats
{
    [JsonConverter(typeof(JsonStringEnumConverter<SenderAction>))]
    public enum SenderAction
    {
        [JsonPropertyName("typing_on")]
        TypingOn,

        [JsonPropertyName("sending_photo")]
        SendingPhoto,

        [JsonPropertyName("sending_video")]
        SendingVideo,

        [JsonPropertyName("sending_audio")]
        SendingAudio,

        [JsonPropertyName("sending_file")]
        SendingFile,

        [JsonPropertyName("mark_seen")]
        MarkSeen
    }
}
