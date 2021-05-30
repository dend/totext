using System.Text.Json.Serialization;

namespace ToText.Plugin.ACS.Models
{
    public class Subscription
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
    }
}
