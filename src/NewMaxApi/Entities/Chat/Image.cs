using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class Image
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
