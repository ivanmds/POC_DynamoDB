using Core.Device.Configurations;
using Core.Device.Tests.IntegratedTests;
using Xunit;

namespace Core.Device.Tests.CollectionFixture
{
    [CollectionDefinition(Name)]
    public class HttpClientCollection : ICollectionFixture<HttpClientFixture>
    {
        public const string Name = "Http Collection";
    }
}
