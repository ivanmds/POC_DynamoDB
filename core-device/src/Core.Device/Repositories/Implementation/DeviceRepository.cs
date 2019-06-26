using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Core.Device.Configurations;
using Core.Device.Models;
using Core.Device.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Device.Repositories.Implementation
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        private readonly DeviceContext _context;

        public const string TableName = "Device";
        private const string DeviceKeyIndex = "CustomerId-Key-Index";

        public DeviceRepository(DeviceContext context, IAmazonDynamoDB dynamoDB)
        {
            _context = context;
            _dynamoDB = dynamoDB;
        }

        public async Task<List<DeviceData>> ListAsync(Expression<Func<DeviceData, bool>> query, CancellationToken cancellationToken = default)
        {
            var response = await Task.Run(() => _context.FromQuery(query).Exec().ToList());
            return response;
        }

        public async Task<List<DeviceData>> GetAll(int quantity, CancellationToken cancellationToken = default)
        {
            var response = await Task.Run(() => _context.FromScan<DeviceData>().Exec(quantity).ToList());
            return response;
        }

        public async Task<DeviceData> SelectAsync(Expression<Func<DeviceData, bool>> query, CancellationToken cancellationToken = default)
        {
            var response = await Task.Run(() => _context.FromQuery(query).Exec(1).FirstOrDefault());
            return response;
        }

        public async Task<string> GetPushTokenAsync(int customerId, CancellationToken cancellationToken)
        {
            string pushToken = null;

            var request = new QueryRequest(TableName)
            {
                IndexName = DeviceKeyIndex,
                KeyConditionExpression = "CustomerId = :customerId",
                FilterExpression = "IsActive = :isActive",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    { ":customerId", new AttributeValue() { N = customerId.ToString() } },
                    { ":isActive", new AttributeValue() { BOOL = true } }
                }
            };

            QueryResponse response = await _dynamoDB.QueryAsync(request, cancellationToken);
            var item = response.Items?.FirstOrDefault();

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK && item?.Count > 0)
                pushToken = item["PushToken"].S;

            return pushToken;
        }

        public async Task<DeviceData> SaveAsync(DeviceData device)
        {
            return await Task.Run(() => _context.PutItem(device, true));
        }

        public async Task RegisterTablesAsync()
        {
            var request = new CreateTableRequest
            {
                TableName = TableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition("Id", ScalarAttributeType.S),
                    new AttributeDefinition("CustomerId", ScalarAttributeType.N)
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement("Id", KeyType.HASH),
                    new KeySchemaElement("CustomerId", KeyType.RANGE)
                },
                ProvisionedThroughput = new ProvisionedThroughput(10, 5),
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>()
                {
                    new GlobalSecondaryIndex()
                    {
                         IndexName = DeviceKeyIndex,
                         KeySchema = new List<KeySchemaElement>()
                         {
                              new KeySchemaElement("CustomerId", KeyType.HASH),
                              new KeySchemaElement("Id", KeyType.RANGE)
                         },
                         ProvisionedThroughput = new ProvisionedThroughput(10,5),
                         Projection = new Projection()
                         {
                            ProjectionType = ProjectionType.INCLUDE,
                            NonKeyAttributes = new List<string>() { "PushToken", "IsActive" }
                         }
                    }
                }
            };

            var tables = await _dynamoDB.ListTablesAsync();

            if (!tables.TableNames.Contains(TableName))
                await _dynamoDB.CreateTableAsync(request);
        }
    }
}
