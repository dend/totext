using System.IO;
using System.Reflection;
using System.Text.Json;
using ToText.Plugin.ACS.Models;

namespace ToText.Plugin.ACS.Helpers
{
    internal class MetaHelper
    {
        private const string SubscriptionFile = "subscription.json";

        internal static Subscription LoadSubscriptionData()
        {
            var configurationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SubscriptionFile);
            using StreamReader reader = new(configurationPath);
            string json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Subscription>(json);
        }
    }
}
