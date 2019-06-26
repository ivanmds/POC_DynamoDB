using Newtonsoft.Json;

namespace Core.Device.Tests.IntegratedTests.ODatas
{
    public class ODataError
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
