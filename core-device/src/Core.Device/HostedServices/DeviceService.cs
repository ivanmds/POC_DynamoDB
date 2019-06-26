using Acesso.Library.Abstractions;
using Acesso.MessageBus;
using AutoMapper;
using Core.Device.Models;
using Core.Device.Models.Input;
using Core.Device.Models.Output;
using Core.Device.Repositories.Interfaces;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewRelic.Api.Agent;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Device.HostedServices
{
    public class DeviceService : QueueServiceBase<DeviceInput, Result<DeviceOutput>>
    {
        private readonly ILogger<DeviceInput> _logger;
        public DeviceService(IServiceProvider serviceProvider, IMessageBusConsumer<DeviceInput> rabbitMessageBusConsumer, IMessageBusPublisher<Result<DeviceOutput>> rabbitMessageBusPublisher, ILogger<DeviceInput> logger) : base(serviceProvider, rabbitMessageBusConsumer, rabbitMessageBusPublisher)
        {
            _logger = logger;
        }

        [Transaction]
        public override async Task Consume(DeviceInput msg, MessageProperties messageProperties, CancellationTokenSource cancellationTokenSource)
        {
            var (isValid, errorType) = Validate(msg);
            
            if(!isValid)
            { 
                await _rabbitMessageBusPublisher.PublishAsync(Result.Fail<DeviceOutput>(errorType.Code, errorType.Message), messageProperties);
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var _mapper = scope.ServiceProvider.GetService<IMapper>();
                var _deviceRepository = scope.ServiceProvider.GetService<IDeviceRepository>();
                var device = await _deviceRepository.SelectAsync(p => p.Id == msg.DeviceId && p.CustomerId == msg.CustomerId && p.IsActive);

                if (device == null)
                {
                    device = _mapper.Map<DeviceData>(msg);
                }

                await _deviceRepository.SaveAsync(device);
                var deviceOutput = _mapper.Map<DeviceOutput>(device);

                await _rabbitMessageBusPublisher.PublishAsync(Result.Ok(deviceOutput), messageProperties);
            }
        }

        public (bool, ErrorType) Validate(DeviceInput model)
        {
            if (string.IsNullOrWhiteSpace(model.DeviceId)) return (false, ErrorCode.IsNullOrWhiteSpaceDeviceIdStatus);
            if (string.IsNullOrWhiteSpace(model.PushToken)) return (false, ErrorCode.IsNullOrWhiteSpacePushTokenStatus);
            return string.IsNullOrWhiteSpace(model.MacAddress) ? (false, ErrorCode.IsNullOrWhiteSpaceMacAddressStatus) : (true, null);
        }
    }
}
