using NewMaxApi.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewMaxApi.Entities
{
    [JsonConverter(typeof(ButtonConverter))]
    public abstract class Button
    {
        [JsonPropertyName("type")]
        public string ButtonType { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        [MaxLength(128)]
        public string ButtonText { get; set; } = string.Empty;
    }

    public class ButtonCallback : Button
    {
        [JsonPropertyName("payload")]
        [MaxLength(1024)]
        public string Payload { get; set; }

        [JsonPropertyName("intent")]
        [DefaultValue("default")]
        public Intent Intent { get; set; }
    }

    public class ButtonLink : Button
    {
        [JsonPropertyName("url")]
        [MaxLength(128)]
        public string Url { get; set; }
    }

    public class ButtonRequestGeoLocation : Button
    {
        [JsonPropertyName("quick")]
        public bool Quick {  get; set; }
    }

    public class ButtonRequestContact: Button {}

    public class ButtonOpenApp : Button
    {
        [JsonPropertyName("web_app")]
        public string WebApp { get; set; }

        [JsonPropertyName("contact_id")]
        public long ContactId { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }

    public class ButtonMessage : Button { }
}


