using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Serialization
{
    public class JsonHelper
    {
        public static string Serialize(object classObject, int maxDepth = int.MaxValue)
        {
            return JsonConvert.SerializeObject(classObject, Formatting.Indented, new JsonSerializerSettings() { MaxDepth = maxDepth });
        }

        public static ResourceT Deserilize<ResourceT>(string jsonText, int maxDepth = int.MaxValue)
        {
            return JsonConvert.DeserializeObject<ResourceT>(jsonText, new JsonSerializerSettings() { MaxDepth = maxDepth });
        }

        public static ResourceT Deserilize<ResourceT>(dynamic classObject, int maxDepth = int.MaxValue)
        {
            var jsonText = JsonConvert.SerializeObject(classObject, Formatting.Indented, new JsonSerializerSettings() { MaxDepth = maxDepth });
            return JsonConvert.DeserializeObject<ResourceT>(jsonText, new JsonSerializerSettings() { MaxDepth = maxDepth });
        }
    }
}
