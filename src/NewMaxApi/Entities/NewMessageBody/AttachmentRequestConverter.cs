using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities
{
    public class AttachmentRequestConverter : JsonConverter<AttachmentRequest>
    {
        public override AttachmentRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeElement))
                throw new JsonException("Missing 'type' in markup element");

            var type = typeElement.GetString()?.ToLowerInvariant() ?? "";

            return type switch
            {
                "image" => JsonSerializer.Deserialize<AttachmentRequestImage>(root, options)!,
                "video" => JsonSerializer.Deserialize<AttachmentRequestVideo>(root, options)!,
                "audio" => JsonSerializer.Deserialize<AttachmentRequestAudio>(root, options)!,
                "file" => JsonSerializer.Deserialize<AttachmentRequestFile>(root, options)!,
                "sticker" => JsonSerializer.Deserialize<AttachmentRequestSticker>(root, options)!,
                "contact" => JsonSerializer.Deserialize<AttachmentRequestContact>(root, options)!,
                "inline_keyboard" => JsonSerializer.Deserialize<AttachmentRequestInlineKeyboard>(root, options)!,
                "location" => JsonSerializer.Deserialize<AttachmentRequestLocation>(root, options)!,
                "share" => JsonSerializer.Deserialize<AttachmentRequestShare>(root, options)!,
                _ => throw new JsonException($"Unknown markup element type: {type}")
            };
        }

        public override void Write(Utf8JsonWriter writer, AttachmentRequest value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
