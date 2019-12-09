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
                var dynamoConfig = configuration.GetSection("DynamoDB").Get<DynamoDBConfiguration>();

                services.AddSingleton<IAmazonDynamoDB>(x =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoConfig.ServiceURL };

                    return new AmazonDynamoDBClient(dynamoConfig.AccessKey, dynamoConfig.SecretKey, clientConfig);
                });
            }
            else
                services.AddSingleton<IAmazonDynamoDB>(x => new AmazonDynamoDBClient());


            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            services.AddSingleton<IRegisterTables, RegisterTables>();

            if (hostingEnvironment.IsProduction())
                services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        }
    }
}
