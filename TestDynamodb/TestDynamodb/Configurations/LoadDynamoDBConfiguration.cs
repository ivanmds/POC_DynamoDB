using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestDynamodb.Repositories;
using TestDynamodb.Repositories.Interfaces;

namespace TestDynamodb.Configurations
{
    public static class LoadDynamoDBConfiguration
    {
        public static void AddDynamoDB(this IServiceCollection services, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            if (!hostingEnvironment.IsProduction())
            {
                services.AddScoped<IAmazonDynamoDB>(x =>
                {
                    var dynamoConfig = configuration.GetSection("ConnectionStrings:DynamoDB").Get<DynamoDBConfiguration>();
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoConfig.ServiceURL };

                    return new AmazonDynamoDBClient(dynamoConfig.AccessKey, dynamoConfig.SecretKey, clientConfig);
                });
            }
            else
                services.AddScoped<IAmazonDynamoDB>(x => new AmazonDynamoDBClient());


            services.AddScoped<IDynamoDBContext, DynamoDBContext>();
            services.AddScoped<IRegisterTables, RegisterTables>();

            if (hostingEnvironment.IsProduction())
                services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        }
    }
}
