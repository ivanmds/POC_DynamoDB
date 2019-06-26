using Core.Device.Models;
using Core.Device.Tests.CollectionFixture;
using Core.Device.Tests.IntegratedTests.ODatas;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Core.Device.Tests.IntegratedTests.V1
{

    [Collection("Http Collection")]
    public class DeviceControllerIntegratedTests
    {
        private readonly IFlurlClient _flurlClient;

        public DeviceControllerIntegratedTests(HttpClientFixture http)
        {
            _flurlClient = http.Client;
        }

        [Trait("DeviceControllerIntegratedTests", "Get")]
        [Fact(DisplayName = "Precisa retornar mais que um registro, filtro por key")]
        public async Task ShouldReturnMoreThenOne_WhenFilterByKey()
        {
            var deviceId = "1";
            var result = await $"api/v1/Device?key={deviceId}"
                .WithClient(_flurlClient)
                .WithHeader("X-Correlation-ID", "teste")
                .GetStringAsync();

            var devices = JsonConvert.DeserializeObject<List<DeviceData>>(result);
            Assert.True(devices.Count > 0);
        }



        [Trait("DeviceControllerIntegratedTests", "Get")]
        [Fact(DisplayName = "Precisa retornar nenhum registro, filtro por key")]
        public async Task ShouldReturnZero_WhenFilterByKey()
        {
            var deviceId = "5";
            var result = await $"api/v1/Device?key={deviceId}"
               .WithClient(_flurlClient)
                .WithHeader("X-Correlation-ID", "teste")
                .GetStringAsync();

            var devices = JsonConvert.DeserializeObject<List<DeviceData>>(result);
            Assert.Null(devices);
        }



        [Trait("DeviceControllerIntegratedTests", "Get")]
        [Fact(DisplayName = "Retorna o pushToken do device com customerId")]
        public async Task ShouldReturnPushToken_ForCustomerIdExistent()
        {
            var result = await $"api/v1/Device/pushtoken?customerId={BasicData.Device1.CustomerId}"
                 .WithClient(_flurlClient)
                .WithHeader("X-Correlation-ID", "teste")
                .GetStringAsync();

            Assert.NotNull(result);
            Assert.Equal(BasicData.Device1.PushToken, result);
        }

        [Trait("DeviceControllerIntegratedTests", "Get")]
        [Fact(DisplayName = "Retorna o erro  do device, filtro por customerId")]
        public async Task ShouldReturnError_ForCustomerIdNotExistent()
        {
            var result = await $"api/v1/Device/PushToken?customerId=0"
               .WithClient(_flurlClient)
               .WithHeader("X-Correlation-ID", "teste")
               .AllowHttpStatus(HttpStatusCode.NotFound)
               .GetStringAsync();

            dynamic httpResult = JsonConvert.DeserializeObject(result);
            Assert.True(httpResult.status == 404);
        }

    }

}
