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

        [Fact]
        public async Task SaveAndGetIndexUserCard()
        {
            //arrange
            var card = new Card
            {
                CompanyKey = "Acesso",
                ActivateCode = Guid.NewGuid().ToString(),
                DocumentNumber = Guid.NewGuid().ToString(),
                BankAgency = "0001",
                BanckAccount = "123456"
            };
            card.LoadIds();

            //act
            await _repository.SaveAsync(card);
            var cardFound = await _repository.GetIndexUserCardAsync(card.IndexUserCard);

            //assert
            Assert.NotNull(cardFound);
        }

        [Fact]
        public async Task SaveAndGetIndexUserCard2()
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

            cardFound.DocumentNumber = Guid.NewGuid().ToString();
            cardFound.BankAgency = "0001";
            cardFound.BanckAccount = "123456";
            cardFound.LoadIds();

            await _repository.SaveAsync(cardFound);
            var cardFound2 = await _repository.GetIndexUserCardAsync(cardFound.IndexUserCard);

            //assert
            Assert.NotNull(cardFound2);
        }


        [Fact]
        public async Task SaveAndGetIndexUserCard3()
        {
            //arrange
            var card = new Card
            {
                CompanyKey = "Acesso",
                ActivateCode = Guid.NewGuid().ToString(),
                DocumentNumber = Guid.NewGuid().ToString(),
                BankAgency = "0001",
                BanckAccount = "123456",
            };
            card.LoadIds();

            //act
            await _repository.SaveAsync(card);
            var cardFound = await _repository.GetIndexUserCardAsync(card.IndexUserCard, card.CardId);

            //assert
            Assert.NotNull(cardFound);
        }

    }
}
