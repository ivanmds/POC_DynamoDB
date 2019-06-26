using HealthChecks.DynamoDb;

namespace Core.Device.Configurations
{
    public class DynamoDBConfiguration : DynamoDBOptions
    {
        public string ServiceURL { get; set; }
    }
}
