using HealthChecks.DynamoDb;

namespace TestDynamodb.Configurations
{
    public class DynamoDBConfiguration : DynamoDBOptions
    {
        public string ServiceURL { get; set; }
    }
}
