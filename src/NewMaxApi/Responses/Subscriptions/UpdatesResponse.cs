using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Subscriptions
{
    public class UpdatesResponse
    {
        [JsonPropertyName("updates")]
        public Update[] Updates { get; set; }

        [JsonPropertyName("marker")]
        public long? Marker { get; set; }
    }
}
