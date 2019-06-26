using Flurl.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;

namespace Core.Device.Tests.CollectionFixture
{
    public class HttpClientFixture : IDisposable
    {
        private readonly TestServer _server;
        public IFlurlClient Client { get; private set; }

        public HttpClientFixture()
        {
            _server = new TestServer(new WebHostBuilder().UseEnvironment("Test").ConfigureAppConfiguration((hostContext, configApp) =>
            {
                hostContext.Configuration["ConnectionStrings:DynamoDB:ServiceURL"] = "http://localhost:8000";
                hostContext.Configuration["ConnectionStrings:DynamoDB:AccessKey"] = "root";
                hostContext.Configuration["ConnectionStrings:DynamoDB:SecretKey"] = "secret";

            }).UseStartup<StartupTest>());

            Client = new FlurlClient(_server.CreateClient());
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
