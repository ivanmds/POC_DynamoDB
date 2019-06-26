using Amazon.DynamoDBv2;
using Core.Device.Configurations;
using Core.Device.Repositories.Implementation;
using Core.Device.Repositories.Interfaces;
using Core.Device.Tests.IntegratedTests;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ServiceStack.Aws.DynamoDb;

namespace Core.Device.Tests
{
    public class StartupTest
    {

        public StartupTest(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DeviceContext>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            var dynamoDBConfiguration = Configuration.GetSection("ConnectionStrings:DynamoDB").Get<DynamoDBConfiguration>();


            services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(dynamoDBConfiguration.AccessKey, dynamoDBConfiguration.SecretKey,
                new AmazonDynamoDBConfig { ServiceURL = dynamoDBConfiguration.ServiceURL }));

            services.AddSingleton<IPocoDynamo, DeviceContext>();

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var appDb = scopedServices.GetRequiredService<DeviceContext>();

                BasicData.PopulateTestData(appDb);
            }

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddOData().EnableApiVersioning();
            services.AddMvc(op =>
            {
                op.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                 .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     options.SerializerSettings.Converters.Add(new StringEnumConverter());
                     options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                 });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, VersionedODataModelBuilder modelBuilder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
