using NewMaxApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities
{
    public class UpdateConverter : JsonConverter<Update>
    {
        public override Update Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("update_type", out var typeElement))
                throw new JsonException("Missing 'update_type' in update");

            var updateType = typeElement.GetString()?.ToLowerInvariant() ?? "";

            return updateType switch
            {
                "message_created" => JsonSerializer.Deserialize<MessageCreated>(root, options)!,
                "message_callback" => JsonSerializer.Deserialize<MessageCallback>(root, options)!,
                "message_edited" => JsonSerializer.Deserialize<MessageEdited>(root, options)!,
                "message_removed" => JsonSerializer.Deserialize<MessageRemoved>(root, options)!,
                "bot_added" => JsonSerializer.Deserialize<BotAdded>(root, options)!,
                "bot_removed" => JsonSerializer.Deserialize<BotRemoved>(root, options)!,
                "dialog_muted" => JsonSerializer.Deserialize<DialogMuted>(root, options)!,
                "dialog_unmuted" => JsonSerializer.Deserialize<DialogUnmuted>(root, options)!,
                "dialog_cleared" => JsonSerializer.Deserialize<DialogCleared>(root, options)!,
                "dialog_removed" => JsonSerializer.Deserialize<DialogRemoved>(root, options)!,
                "user_added" => JsonSerializer.Deserialize<UserAdded>(root, options)!,
                "user_removed" => JsonSerializer.Deserialize<UserRemoved>(root, options)!,
                "bot_started" => JsonSerializer.Deserialize<BotStarted>(root, options)!,
                "bot_stopped" => JsonSerializer.Deserialize<BotStopped>(root, options)!,
                "chat_title_changed" => JsonSerializer.Deserialize<ChatTitleChanged>(root, options)!,
                "message_chat_created" => JsonSerializer.Deserialize<MessageChatCreated>(root, options)!,
                _ => throw new JsonException($"Unsupported update_type: {updateType}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Update value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
