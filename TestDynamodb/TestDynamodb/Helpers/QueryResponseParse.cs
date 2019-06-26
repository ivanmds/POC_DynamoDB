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
                properties.Add(GetKeyValue(keyValue));

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
