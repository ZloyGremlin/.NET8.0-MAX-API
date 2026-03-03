using NewMaxApi.Entities;
using System.Text.Json.Serialization;

namespace NewMaxApi.Responses.Subscriptions
{
    public class GetSubscriptionsResponse
    {
        [JsonPropertyName("subscriptions")]
        public Subscription[] Subscriptions { get; set; }
    }
}
