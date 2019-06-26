using Acesso.MessageBus;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Device.HostedServices
{

    public abstract class QueueServiceBase<TInput, TOutput> : BackgroundService where TInput : class where TOutput : class
    {
        protected readonly IServiceProvider _serviceProvider;

        protected readonly IMessageBusConsumer<TInput> _rabbitMessageBusConsumer;
        protected readonly IMessageBusPublisher<TOutput> _rabbitMessageBusPublisher;

        protected QueueServiceBase(IServiceProvider serviceProvider, IMessageBusConsumer<TInput> rabbitMessageBusConsumer, IMessageBusPublisher<TOutput> rabbitMessageBusPublisher)
        {
            _serviceProvider = serviceProvider;
            _rabbitMessageBusConsumer = rabbitMessageBusConsumer;
            _rabbitMessageBusPublisher = rabbitMessageBusPublisher;
        }

        [ExcludeFromCodeCoverage]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            => await Task.Run(() => _rabbitMessageBusConsumer.RegisterHandler(Consume), stoppingToken);

        public abstract Task Consume(TInput msg, MessageProperties messageProperties, CancellationTokenSource cancellationTokenSource);
    }
}
