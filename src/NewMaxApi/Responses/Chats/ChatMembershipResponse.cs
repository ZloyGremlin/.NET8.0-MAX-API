using NewMaxApi.Entities;
using NewMaxApi.Enums;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Chats
{
    public class ChatMembershipResponse
    {
        public ChatMember Membership { get; set; }
    }
}
