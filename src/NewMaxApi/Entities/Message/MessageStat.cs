using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    public class MessageStat
    {
        [JsonPropertyName("views")]
        public int Views { get; set; }
    }
}
