using System.Text.Json.Serialization;

namespace NewMaxApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<ChatPermission>))]
    public enum ChatPermission
    {
        [JsonPropertyName("read_all_messages")]
        ReadAllMessages,

        [JsonPropertyName("add_remove_members")]
        AddRemoveMembers,

        [JsonPropertyName("add_admins")]
        AddAdmins,

        [JsonPropertyName("change_chat_info")]
        ChangeChatInfo,

        [JsonPropertyName("pin_message")]
        PinMessage,

        [JsonPropertyName("write")]
        Write,

        [JsonPropertyName("edit_link")]
        EditLink
    }
}
