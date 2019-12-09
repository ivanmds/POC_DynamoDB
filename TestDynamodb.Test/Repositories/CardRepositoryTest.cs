using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Threading.Tasks;
using TestDynamodb.Models;
using TestDynamodb.Repositories;
using Xunit;

namespace TestDynamodb.Test.Repositories
{
    public class CardRepositoryTest
    {
        private CardRepository _repository;

        public CardRepositoryTest()
        {
            var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
            var dynamoDB = new AmazonDynamoDBClient("root", "secret", clientConfig);
            var context = new DynamoDBContext(dynamoDB);
            _repository = new CardRepository(context);
        }

        [Fact]
        public async Task SaveAndGet()
        {
            //arrange
            var card = new Card
            {
                CompanyKey = "Acesso",
                ActivateCode = Guid.NewGuid().ToString()
            };
            card.LoadIds();

            //act
            await _repository.SaveAsync(card);
            var cardFound = await _repository.GetAsync(card.CardId);

            //assert
            Assert.NotNull(cardFound);
        }

    }
}
