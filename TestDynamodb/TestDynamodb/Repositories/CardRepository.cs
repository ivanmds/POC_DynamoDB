using Amazon.DynamoDBv2.DataModel;
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
    }
}
