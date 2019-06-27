using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDynamodb.Repositories;
using TestDynamodb.Repositories.Interfaces;
using Xunit;

namespace TestDynamodb.Test.Repositories
{
    public class EventRepositoryTest : IDisposable
    {
        private readonly IEventRepository _repository;
        private readonly IAmazonDynamoDB _dynamoDB;
        public EventRepositoryTest()
        {
            var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
            _dynamoDB = new AmazonDynamoDBClient("root", "secret", clientConfig);
            _repository = new EventRepository(_dynamoDB);
        }

        [Fact]
        public async Task Event_List_Sucess()
        {
            //arrange
            await AddEventAsync();

            //act
            var events = await _repository.ListEventsAsync("123456789");

            //assert
            Assert.True(events.Count() > 0);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        private async Task AddEventAsync()
        {
            var @event = new Dictionary<string, AttributeValue>();
            @event.Add("Id", new AttributeValue(Guid.NewGuid().ToString()));
            @event.Add("CreatedAt", new AttributeValue(DateTime.Now.ToString()));
            @event.Add("Account", new AttributeValue("123456789"));
            @event.Add("CustomerId", new AttributeValue("customerId 123456"));
            @event.Add("Description", new AttributeValue("description test"));
            @event.Add("ClientID_1", new AttributeValue { N = "12.55" });
        
        
            //Modelo para conter um objeto filho
            var dataEvent = new Dictionary<string, AttributeValue>();
            dataEvent.Add("DDD", new AttributeValue("11"));
            dataEvent.Add("Numero", new AttributeValue("123456789"));



            @event.Add("Telefone", new AttributeValue { M = dataEvent });

            @event.Add("Telefones", new AttributeValue { L = new List<AttributeValue> { new AttributeValue { M = dataEvent }, new AttributeValue { M = dataEvent } } });


            await _dynamoDB.PutItemAsync(RegisterTables.TABLE_NAME_EVENT, @event);
        }
    }
}
