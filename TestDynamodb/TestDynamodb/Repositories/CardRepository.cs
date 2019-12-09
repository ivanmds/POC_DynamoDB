using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDynamodb.Models;

namespace TestDynamodb.Repositories
{
    public class CardRepository
    {
        private readonly IDynamoDBContext _dBContext;

        public CardRepository(IDynamoDBContext dBContext)
            => _dBContext = dBContext;

        public async Task<Card> GetAsync(string id)
        {
            return await _dBContext.LoadAsync<Card>(id);
        }

        public async Task SaveAsync(Card card)
        {
            await _dBContext.SaveAsync(card);
        }

        public async Task<List<Card>> GetIndexUserCardAsync(string indexCard)
        {
            var queryAsync = _dBContext.FromQueryAsync<Card>(QueryIndexUserCard(indexCard));
            return await queryAsync.GetNextSetAsync();
        }

        public async Task<Card> GetIndexUserCardAsync(string indexCard, string cardId)
        {
            var queryAsync = _dBContext.FromQueryAsync<Card>(QueryIndexUserCard(indexCard, cardId));
            var result = await queryAsync.GetNextSetAsync();
            return result?.FirstOrDefault();
        }


        private QueryOperationConfig QueryIndexUserCard(string id, string cardId = null)
        {
            var query = new QueryOperationConfig
            {
                IndexName = RegisterTables.INDEX_FIND_CARD,
                Filter = new QueryFilter("IndexUserCard", QueryOperator.Equal, new List<AttributeValue> { new AttributeValue { S = id } })
            };

            if (string.IsNullOrEmpty(cardId) == false)
                query.Filter.AddCondition("CardId", QueryOperator.Equal, new List<AttributeValue> { new AttributeValue { S = cardId } });

            return query;
        }

    }
}
