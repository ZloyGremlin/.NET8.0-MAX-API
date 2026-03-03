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
    public class ButtonConverter : JsonConverter<Button>
    {
        public override Button? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            if (!root.TryGetProperty("type", out var typeProp))
                throw new JsonException("Missing 'type' in button");

            var type = typeProp.GetString()?.ToLowerInvariant();

            return type switch
            {
                "callback" => JsonSerializer.Deserialize<ButtonCallback>(root, options),
                "link" => JsonSerializer.Deserialize<ButtonLink>(root, options),
                "request_geo_location" => JsonSerializer.Deserialize<ButtonRequestGeoLocation>(root, options),
                "request_contact" => JsonSerializer.Deserialize<ButtonRequestContact>(root, options),
                "message" => JsonSerializer.Deserialize<ButtonMessage>(root, options),
                "open_app" => JsonSerializer.Deserialize<ButtonOpenApp>(root, options),
                _ => throw new JsonException($"Unknown button type: {type}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Button value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
