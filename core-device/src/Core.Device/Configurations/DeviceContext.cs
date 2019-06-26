using Amazon.DynamoDBv2;
using Core.Device.Models;
using ServiceStack.Aws.DynamoDb;

namespace Core.Device.Configurations
{
    public class DeviceContext : PocoDynamo
    {
        public DeviceContext(IAmazonDynamoDB client) : base(client)
        {
        }

        public void RegisterTables()
        {
            ClientWith().RegisterTable<DeviceData>();
            ClientWith().InitSchema();
        }

    }
}
