using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities
{
    public class AttachmentConverter : JsonConverter<Attachment>
    {
        public override Attachment Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeElement))
                throw new JsonException("Missing 'type' in attachment");

            var type = typeElement.GetString()?.ToLowerInvariant() ?? "";

            return type switch
            {
                "image" => JsonSerializer.Deserialize<AttachmentImage>(root, options)!,
                "video" => JsonSerializer.Deserialize<AttachmentVideo>(root, options)!,
                "audio" => JsonSerializer.Deserialize<AttachmentAudio>(root, options)!,
                "file" => JsonSerializer.Deserialize<AttachmentFile>(root, options)!,
                "sticker" => JsonSerializer.Deserialize<AttachmentStiker>(root, options)!,
                "contact" => JsonSerializer.Deserialize<AttachmentContact>(root, options)!,
                "inline_keyboard" => JsonSerializer.Deserialize<AttachmentInlineKeyboard>(root, options)!,
                "location" => JsonSerializer.Deserialize<AttachmentLocation>(root, options)!,
                "share" => JsonSerializer.Deserialize<AttachmentShare>(root, options)!,
                _ => throw new JsonException($"Unsupported attachment type: {type}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Attachment value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
