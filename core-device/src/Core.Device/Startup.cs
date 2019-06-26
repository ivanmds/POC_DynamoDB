using Acesso.Library.Abstractions;
using Acesso.MessageBus.RabbitMQ;
using Amazon.DynamoDBv2;
using AutoMapper;
using Core.Device.Configurations;
using Core.Device.HostedServices;
using Core.Device.MapperProfiles;
using Core.Device.Models.Input;
using Core.Device.Models.Output;
using Core.Device.Repositories.Implementation;
using Core.Device.Repositories.Interfaces;
using EasyNetQ;
using HealthChecks.UI.Client;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ServiceStack.Aws.DynamoDb;
using System.Diagnostics.CodeAnalysis;

namespace Core.Device
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DeviceContext>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });

            if (!HostingEnvironment.IsDevelopment())
                services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

            var runLocalDynamoDb = Configuration.GetSection("LocalMode").Get<bool>();
            var dynamoDBConfiguration = Configuration.GetSection("ConnectionStrings:DynamoDB").Get<DynamoDBConfiguration>();

            if (runLocalDynamoDb)
                services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(dynamoDBConfiguration.AccessKey, dynamoDBConfiguration.SecretKey,
                    new AmazonDynamoDBConfig { ServiceURL = dynamoDBConfiguration.ServiceURL }));
            else
                services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient());
            services.AddSingleton<IPocoDynamo, DeviceContext>();

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var deviceRepository = scopedServices.GetRequiredService<IDeviceRepository>();
                deviceRepository.RegisterTablesAsync().Wait();
            }

            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DeviceDataProfile());
            });

            services.AddSingleton(x => config.CreateMapper());

            services.AddSingleton(x => RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ")).Advanced);

            services.AddHealthChecks()
                   .AddRabbitMQ(Configuration.GetConnectionString("RabbitMQ"));


            var rabbitMQConfigs = new RabbitConfig();
            Configuration.Bind("RabbitMQ", rabbitMQConfigs);
            services.AddSingleton(rabbitMQConfigs);

            var exchangeTypes = ExchangeTypes.topic;
            services.AddMessageBusPublisher<Result<DeviceOutput>>(new ExchangeConfiguration(exchangeTypes, rabbitMQConfigs.ExchangeQueueDevice), new QueueConfiguration(rabbitMQConfigs.OutputQueueDevice, false, rabbitMQConfigs.RoutingKeyOutputDevice));
            services.AddMessageBusConsumer<DeviceInput>(new ExchangeConfiguration(exchangeTypes, rabbitMQConfigs.ExchangeQueueDevice), new QueueConfiguration(rabbitMQConfigs.InputQueueDevice, false, rabbitMQConfigs.RoutingKeyInputDevice));

            services.AddOData().EnableApiVersioning();

            services.AddMvc(
                op =>
                {
                    op.EnableEndpointRouting = false;
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            if (Configuration["environment"] != "Test")
                services.AddHostedService<DeviceService>();
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, VersionedODataModelBuilder modelBuilder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks(new PathString("/hc"), new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseSimpleLogRequest();

            var models = modelBuilder.GetEdmModels();
            app.UseMvc(b =>
            {
                b.Select()
                 .Expand()
                 .Filter()
                 .OrderBy()
                 .MaxTop(100)
                 .Count();

                b.MapVersionedODataRoutes("odata", "v{version:apiVersion}", models);
            });
        }
    }
}
