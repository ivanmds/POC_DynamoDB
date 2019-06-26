using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TestDynamodb.Helpers
{
    public static class QueryResponseParse
    {
        public static IEnumerable<object> GetListItems(QueryResponse response)
        {
            return GetListItems<object>(response);
        }
        public static IEnumerable<T> GetListItems<T>(QueryResponse response)
        {
            if (response?.Items?.Count > 0)
                foreach (Dictionary<string, AttributeValue> item in response.Items)
                    yield return GetItem<T>(item);
        }

        public static T GetItem<T>(Dictionary<string, AttributeValue> item)
        {
            List<JProperty> properties = new List<JProperty>();
            foreach (KeyValuePair<string, AttributeValue> keyValue in item)
            {
                var keyValueParse = GetKeyValue(keyValue);
                properties.Add(new JProperty(keyValueParse.Key, keyValueParse.Value));
            }

            var obj = new JObject(properties);
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        public static KeyValuePair<string, string> GetKeyValue(KeyValuePair<string, AttributeValue> keyValue)
        {
            string key = keyValue.Key;
            string value = null;

            if (!string.IsNullOrEmpty(keyValue.Value.S))
                value = keyValue.Value.S;

            //add more parse if you need.

            return KeyValuePair.Create<string, string>(key, value);
        }
    }
}
