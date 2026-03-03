using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities.Message
{
    public class MarkupElementConverter : JsonConverter<MarkupElement>
    {
        public override MarkupElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeElement))
                throw new JsonException("Missing 'type' in markup element");

            var type = typeElement.GetString()?.ToLowerInvariant() ?? "";

            return type switch
            {
                "underline" => JsonSerializer.Deserialize<MarkupElementUnderline>(root, options)!,
                "strong" => JsonSerializer.Deserialize<MarkupElementStrong>(root, options)!,
                "emphasized" => JsonSerializer.Deserialize<MarkupElementEmphasized>(root, options)!,
                "link" => JsonSerializer.Deserialize<MarkupElementLink>(root, options)!,
                "strikethrough" => JsonSerializer.Deserialize<MarkupElementStrikethrough>(root, options)!,
                "user_mention" => JsonSerializer.Deserialize<MarkupElementUserMention>(root, options)!,
                "heading" => JsonSerializer.Deserialize<MarkupElementHeading>(root, options)!,
                "highlighted" => JsonSerializer.Deserialize<MarkupElementHighlighted>(root, options)!,
                "monospaced" => JsonSerializer.Deserialize<MarkupElementMonospaced>(root, options)!,
                _ => throw new JsonException($"Unknown markup element type: {type}")
            };
        }

        public override void Write(Utf8JsonWriter writer, MarkupElement value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
