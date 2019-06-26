using Newtonsoft.Json;

namespace Core.Device.Tests.IntegratedTests.ODatas
{
    public class OData<T>
    {
        [JsonProperty("odata.context")]
        public string Metadata { get; set; }
        [JsonProperty("value")]
        public T Value { get; set; }
    }
}
