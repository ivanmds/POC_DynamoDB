using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDynamodb.Helpers;
using TestDynamodb.Models;
using TestDynamodb.Repositories.Interfaces;

namespace TestDynamodb.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IAmazonDynamoDB _dynamoDB;

        public EventRepository(IAmazonDynamoDB dynamoDB) => _dynamoDB = dynamoDB;


        public async Task<IEnumerable<Event>> ListEventsAsync(string account)
        {
            QueryRequest request = QueryListEvents(account);
            QueryResponse response = await _dynamoDB.QueryAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return QueryResponseParse.GetListItems<Event>(response);

            return null;
        }

        public void Dispose()
        {
            _dynamoDB.Dispose();
        }

        #region HELPERS METHODS

        private QueryRequest QueryListEvents(string account)
        {
            var request = new QueryRequest(RegisterTables.TABLE_NAME_EVENT)
            {
                IndexName = RegisterTables.INDEX_FIND_LAST_BY_ACCUNT,
                KeyConditionExpression = "Account = :account",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    { ":account", new AttributeValue(account) },
                }
            };

            return request;
        }

        #endregion
    }
}
