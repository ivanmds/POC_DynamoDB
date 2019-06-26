using Acesso.Library.Abstractions;
using Acesso.MessageBus;
using Amazon.DynamoDBv2;
using AutoMapper;
using Core.Device.HostedServices;
using Core.Device.MapperProfiles;
using Core.Device.Models;
using Core.Device.Models.Input;
using Core.Device.Models.Output;
using Core.Device.Repositories.Interfaces;
using Core.Device.Tests.IntegratedTests;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Core.Device.Tests.HostedServicesTests
{
    public class DeviceServiceTest
    {
        readonly IServiceProvider _provider;
        readonly IServiceScope _scope;
        readonly IServiceScopeFactory _factory;
        readonly IDeviceRepository _repo;
        readonly DeviceService _service;
        readonly IMessageBusConsumer<DeviceInput> _rabbitMessageBusConsumer;
        readonly IMessageBusPublisher<Result<DeviceOutput>> _rabbitMessageBusPublisher;

        private const string AcceptJson = "application/json";
        private readonly string _commonAccepter = "application/protobuf";

        public DeviceServiceTest()
        {
            _provider = Substitute.For<IServiceProvider>();
            _scope = Substitute.For<IServiceScope>();
            _factory = Substitute.For<IServiceScopeFactory>();
            _repo = Substitute.For<IDeviceRepository>();
            var _amazonDynamoDb = Substitute.For<IAmazonDynamoDB>();
            var logger = Substitute.For<ILogger<DeviceInput>>();

            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DeviceDataProfile());
            });


            _factory.CreateScope().Returns(_scope);
            _provider.GetService<IServiceScopeFactory>().Returns(_factory);
            _provider.GetService<IMapper>().Returns(config.CreateMapper());
            _provider.GetService<IDeviceRepository>().Returns(_repo);
            _provider.GetService<IAmazonDynamoDB>().Returns(_amazonDynamoDb);
            _scope.ServiceProvider.Returns(_provider);

            _rabbitMessageBusConsumer = Substitute.For<IMessageBusConsumer<DeviceInput>>();
            _rabbitMessageBusPublisher = Substitute.For<IMessageBusPublisher<Result<DeviceOutput>>>();

            _service = new DeviceService(_provider, _rabbitMessageBusConsumer, _rabbitMessageBusPublisher, logger);
        }

        [Trait("DeviceServiceTest", "HappyPath")]
        [Fact(DisplayName = "HappyPath device service")]
        public async Task Ok()
        {
            await _repo.SaveAsync(Arg.Is<DeviceData>(c => c.Id.Equals("1") && c.CustomerId.Equals(1)));


            var deviceInput = BasicData.DataInput;
            var prop = new MessageProperties
            {
                ContentType = AcceptJson,
                CorrelationId = Guid.NewGuid().ToString()
            };


            prop.Headers.Add("accept", _commonAccepter);


            await _service.Consume(deviceInput, prop, Substitute.For<CancellationTokenSource>());
            await _repo.Received(1).SaveAsync(Arg.Any<DeviceData>());
            await _rabbitMessageBusPublisher.Received(1).PublishAsync(Arg.Is<Result<DeviceOutput>>(t => t.Value.DeviceId == deviceInput.DeviceId),
                 Arg.Is<MessageProperties>(t => t.Headers["accept"].Equals("application/protobuf")));
        }

        [Trait("DeviceServiceTest", "Existing device")]
        [Fact(DisplayName = "Existing  device service")]
        public async Task Existing()
        {
            _repo.SelectAsync(p => p.Id == "1" && p.CustomerId == 1)
                .Returns(BasicData.Device1);

            var deviceInput = BasicData.DataInput;
            var prop = new MessageProperties
            {
                ContentType = AcceptJson,
                CorrelationId = Guid.NewGuid().ToString()
            };

            prop.Headers.Add("accept", _commonAccepter);

            await _service.Consume(deviceInput, prop, Substitute.For<CancellationTokenSource>());
            await _repo.Received(1).SelectAsync(Arg.Any<Expression<Func<DeviceData, bool>>>());
            await _repo.Received(1).SaveAsync(Arg.Any<DeviceData>());
            await _rabbitMessageBusPublisher.Received(1).PublishAsync(Arg.Is<Result<DeviceOutput>>(t => t.Value.DeviceId == deviceInput.DeviceId),
                    Arg.Is<MessageProperties>(t => t.Headers["accept"].Equals("application/protobuf")));
        }

        [Trait("DeviceServiceTest", "Null Or WhiteSpace DeviceId")]
        [Fact(DisplayName = "Null Or WhiteSpace DeviceId")]
        public async Task ShouldNullOrWhiteSpace_DeviceId()
        {
            _repo.SelectAsync(p => p.Id == "1" && p.CustomerId == 1)
                .Returns(BasicData.Device1);

            var deviceInput = new DeviceInput { PushToken = "1", CustomerId = 1, MacAddress = "Teste" };
            var prop = new MessageProperties
            {
                ContentType = AcceptJson,
                CorrelationId = Guid.NewGuid().ToString()
            };

            prop.Headers.Add("accept", _commonAccepter);

            await _service.Consume(deviceInput, prop, Substitute.For<CancellationTokenSource>());
            await _repo.Received(0).SelectAsync(Arg.Any<Expression<Func<DeviceData, bool>>>());
            await _repo.Received(0).SaveAsync(Arg.Any<DeviceData>());
            await _rabbitMessageBusPublisher.Received(1).PublishAsync(Arg.Is<Result<DeviceOutput>>(t => !t.IsSuccess),
                    Arg.Is<MessageProperties>(t => t.Headers["accept"].Equals("application/protobuf")));
        }


        [Trait("DeviceServiceTest", "Null Or WhiteSpace PushToken")]
        [Fact(DisplayName = "Null Or WhiteSpace PushToken")]
        public async Task ShouldNullOrWhiteSpace_PushToken()
        {
            _repo.SelectAsync(p => p.Id == "1" && p.CustomerId == 1)
                .Returns(BasicData.Device1);

            var deviceInput = new DeviceInput { DeviceId = "1", CustomerId = 1, MacAddress = "Teste" };
            var prop = new MessageProperties
            {
                ContentType = AcceptJson,
                CorrelationId = Guid.NewGuid().ToString()
            };

            prop.Headers.Add("accept", _commonAccepter);

            await _service.Consume(deviceInput, prop, Substitute.For<CancellationTokenSource>());
            await _repo.Received(0).SelectAsync(Arg.Any<Expression<Func<DeviceData, bool>>>());
            await _repo.Received(0).SaveAsync(Arg.Any<DeviceData>());
            await _rabbitMessageBusPublisher.Received(1).PublishAsync(Arg.Is<Result<DeviceOutput>>(t => !t.IsSuccess),
                    Arg.Is<MessageProperties>(t => t.Headers["accept"].Equals("application/protobuf")));
        }

        [Trait("DeviceServiceTest", "Null Or WhiteSpace MacAddress")]
        [Fact(DisplayName = "Null Or WhiteSpace MacAddress")]
        public async Task ShouldNullOrWhiteSpace_MacAddress()
        {
            _repo.SelectAsync(p => p.Id == "1" && p.CustomerId == 1)
                .Returns(BasicData.Device1);

            var deviceInput = new DeviceInput { DeviceId = "1", CustomerId = 1, PushToken = "Teste" };
            var prop = new MessageProperties
            {
                ContentType = AcceptJson,
                CorrelationId = Guid.NewGuid().ToString()
            };

            prop.Headers.Add("accept", _commonAccepter);

            await _service.Consume(deviceInput, prop, Substitute.For<CancellationTokenSource>());
            await _repo.Received(0).SelectAsync(Arg.Any<Expression<Func<DeviceData, bool>>>());
            await _repo.Received(0).SaveAsync(Arg.Any<DeviceData>());
            await _rabbitMessageBusPublisher.Received(1).PublishAsync(Arg.Is<Result<DeviceOutput>>(t => !t.IsSuccess),
                    Arg.Is<MessageProperties>(t => t.Headers["accept"].Equals("application/protobuf")));
        }

    }
}
