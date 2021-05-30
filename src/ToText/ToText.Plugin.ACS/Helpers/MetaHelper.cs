using System.IO;
using System.Text.Json;
using ToText.Plugin.ACS.Models;

namespace ToText.Plugin.ACS.Helpers
{
    internal class MetaHelper
    {
        private const string SubscriptionFile = "subscription.json";

        internal static Subscription LoadSubscriptionData()
        {
            using StreamReader reader = new(SubscriptionFile);
            string json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Subscription>(json);
        }
    }
}
