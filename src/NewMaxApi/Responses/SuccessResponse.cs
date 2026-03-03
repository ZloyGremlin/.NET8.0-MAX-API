using System.Text.Json.Serialization;

namespace NewMaxApi.Responses
{
    public class SuccessResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
