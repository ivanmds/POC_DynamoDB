using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace TestDynamodb.Helpers
{
    public static class BuildObject
    {
        public static object Get(IEnumerable<KeyValuePair<string, AttributeValue>> item)
        {
            return Get<object>(item);
        }

        public static T Get<T>(IEnumerable<KeyValuePair<string, AttributeValue>> item)
        {
            var obj = GetJObject(item);
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        public static T Get<T>(Dictionary<string, string> item)
        {
            var obj = GetJObject(item);
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        public static JObject GetJObject(IEnumerable<KeyValuePair<string, AttributeValue>> item)
        {
            List<JProperty> properties = new List<JProperty>();
            foreach (KeyValuePair<string, AttributeValue> keyValue in item)
                properties.Add(GetKeyValue(keyValue));

            return new JObject(properties);
        }

        public static JObject GetJObject(Dictionary<string, string> item)
        {
            List<JProperty> properties = new List<JProperty>();
            foreach (KeyValuePair<string, string> keyValue in item)
                properties.Add(new JProperty(keyValue.Key, keyValue.Value));

            return new JObject(properties);
        }

        public static JProperty GetKeyValue(KeyValuePair<string, AttributeValue> keyValue)
        {
            string key = keyValue.Key;
            object value = GetValue(keyValue.Value);

            return new JProperty(key, value);
        }

        public static object GetValue(AttributeValue value)
        {
            object valueReturn = null;

            if (!string.IsNullOrEmpty(value.S))
                valueReturn = value.S;

            else if (!string.IsNullOrEmpty(value.N))
                valueReturn = value.N;

            else if (value?.M?.Count > 0)
            {
                var properties = new List<JProperty>();
                foreach (KeyValuePair<string, AttributeValue> keyValueData in value.M)
                    properties.Add(GetKeyValue(keyValueData));

                valueReturn = new JObject(properties);
            }

            else if (value?.L?.Count > 0)
            {
                var listObj = new List<object>();
                foreach (var item in value.L)
                    listObj.Add(GetValue(item));

                return listObj;
            }

            //add more parse if you need.

            return valueReturn;
        }
    }
}
