using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace TestDynamodb.Helpers
{
    public static class BuildObject
    {
        public static IEnumerable<T> GetList<T>(QueryResponse response)
        {
            if (response?.Items?.Count > 0)
                foreach (Dictionary<string, AttributeValue> item in response.Items)
                    yield return Get<T>(item);
        }

        public static T Get<T>(Dictionary<string, AttributeValue> item)
        {
            return Get<T>(item);
        }

        public static object Get(IEnumerable<KeyValuePair<string, AttributeValue>> item)
        {
            return Get<object>(item);
        }

        public static T Get<T>(IEnumerable<KeyValuePair<string, AttributeValue>> item)
        {
            List<JProperty> properties = new List<JProperty>();
            foreach (KeyValuePair<string, AttributeValue> keyValue in item)
                properties.Add(GetKeyValue(keyValue));

            var obj = new JObject(properties);
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        public static T Get<T>(Dictionary<string, string> item)
        {
            List<JProperty> properties = new List<JProperty>();
            foreach (KeyValuePair<string, string> keyValue in item)
                properties.Add(new JProperty(keyValue.Key, keyValue.Value));

            var obj = new JObject(properties);
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        public static JProperty GetKeyValue(KeyValuePair<string, AttributeValue> keyValue)
        {
            string key = keyValue.Key;
            object value = null;

            if (!string.IsNullOrEmpty(keyValue.Value.S))
                value = keyValue.Value.S;

            else if (keyValue.Value?.M?.Count > 0)
            {
                var properties = new List<JProperty>();
                foreach (KeyValuePair<string, AttributeValue> keyValueData in keyValue.Value.M)
                    properties.Add(GetKeyValue(keyValueData));

                value = new JObject(properties);
            }

            //add more parse if you need.

            return new JProperty(key, value);
        }
    }
}
