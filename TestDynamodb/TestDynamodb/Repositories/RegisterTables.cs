using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDynamodb.Repositories.Interfaces;

namespace TestDynamodb.Repositories
{
    public class RegisterTables : IRegisterTables
    {
        public const string TABLE_NAME_EVENT = "Event";
        public const string TABLE_NAME_PERSON = "Person";
        public const string TABLE_NAME_COMPANY = "Company";
        public const string INDEX_FIND_LAST_BY_ACCUNT = "IndexFindLastByAccount";
        public const string INDEX_FIND_LAST_BY_CUSTOMERID = "IndexFindLastByCustomerId";

        private readonly IAmazonDynamoDB _dynamoDB;
        private ListTablesResponse _tables;
        public RegisterTables(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        public async Task RegisterAsync()
        {
            _tables = await _dynamoDB.ListTablesAsync();
            await CreateTableEvent();
            await CreateTablePerson();
            await CreateTableCompany();
        }

        private async Task CreateTableEvent()
        {
            var request = new CreateTableRequest
            {
                TableName = TABLE_NAME_EVENT,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition("Id", ScalarAttributeType.S),
                    new AttributeDefinition("CreatedAt", ScalarAttributeType.S),
                    new AttributeDefinition("Account", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement("Id", KeyType.HASH),
                    new KeySchemaElement("CreatedAt", KeyType.RANGE),
                },
                ProvisionedThroughput = new ProvisionedThroughput(10, 5),
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>()
                {
                    new GlobalSecondaryIndex()
                    {
                        IndexName = INDEX_FIND_LAST_BY_ACCUNT,
                        KeySchema = new List<KeySchemaElement>()
                        {
                            new KeySchemaElement("Account", KeyType.HASH),
                            new KeySchemaElement("CreatedAt", KeyType.RANGE)
                        },
                        ProvisionedThroughput = new ProvisionedThroughput(10, 5),
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL
                        }
                    }
                }
            };

            if (!_tables.TableNames.Contains(TABLE_NAME_EVENT))
                await _dynamoDB.CreateTableAsync(request);
            else
            {
                var describe = await _dynamoDB.DescribeTableAsync(TABLE_NAME_EVENT);

                //Exemplo de criação de um globalSecondaryIndex em uma tabela já existente.
                if (!describe.Table.GlobalSecondaryIndexes.Any(i => i.IndexName == INDEX_FIND_LAST_BY_CUSTOMERID))
                    await CreateIndexFindLastByCustomerIdAsync(INDEX_FIND_LAST_BY_CUSTOMERID);
            }
        }

        private async Task CreateTablePerson()
        {
            var request = new CreateTableRequest
            {
                TableName = TABLE_NAME_PERSON,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition("Id", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement("Id", KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput(10, 5),
            };

            if (!_tables.TableNames.Contains(TABLE_NAME_PERSON))
                await _dynamoDB.CreateTableAsync(request);
        }

        private async Task CreateTableCompany()
        {
            var request = new CreateTableRequest
            {
                TableName = TABLE_NAME_COMPANY,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition("Id", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement("Id", KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput(10, 5),
            };

            if (!_tables.TableNames.Contains(TABLE_NAME_COMPANY))
                await _dynamoDB.CreateTableAsync(request);
        }

        private async Task CreateIndexFindLastByCustomerIdAsync(string indexName)
        {
            var updateTable = new UpdateTableRequest();
            updateTable.TableName = TABLE_NAME_EVENT;

            updateTable.AttributeDefinitions = new List<AttributeDefinition>
                        {
                            new AttributeDefinition("DataCustomerId", ScalarAttributeType.S),
                            new AttributeDefinition("DataId", ScalarAttributeType.S),
                        };

            updateTable.GlobalSecondaryIndexUpdates = new List<GlobalSecondaryIndexUpdate> {
                            new GlobalSecondaryIndexUpdate
                            {
                                 Create = new CreateGlobalSecondaryIndexAction
                                 {
                                      IndexName = indexName,
                                      KeySchema = new List<KeySchemaElement>
                                      {
                                          new KeySchemaElement("DataCustomerId", KeyType.HASH),
                                          new KeySchemaElement("DataId", KeyType.RANGE)
                                      },
                                      ProvisionedThroughput = new ProvisionedThroughput(10, 5),
                                      Projection = new  Projection
                                      {
                                          ProjectionType = ProjectionType.ALL
                                      }
                                 }
                            }
                        };

            await _dynamoDB.UpdateTableAsync(updateTable);
        }
    }
}
