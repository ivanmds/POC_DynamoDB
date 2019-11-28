using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Threading.Tasks;
using TestDynamodb.Models;
using TestDynamodb.Repositories;
using TestDynamodb.Repositories.Interfaces;
using Xunit;

namespace TestDynamodb.Test.Repositories
{
    public class PersonRepositoryTest
    {
        private IPersonRepository _repository;

        public PersonRepositoryTest()
        {
            var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
            var dynamoDB = new AmazonDynamoDBClient("root", "secret", clientConfig);
            var context = new DynamoDBContext(dynamoDB);
            _repository = new PersonRepository(context);
        }

        [Fact]
        public async Task SaveNewPerson()
        {
            //arrange
            var person = new Person();
            person.Id = Guid.NewGuid().ToString();
            person.Name = "Teste";
            
            person.Phones.Add(new Phone { DDD = "11", Number = "123456789" });
            person.Phones.Add(new Phone { DDD = "11", Number = "987654321" });

            person.Addresses.Add(new Address { City = "SP", State = "São Paulo", Number = "123", Street = "Rua", ZipCode = "1023456" });
            person.Addresses.Add(new Address { City = "BH", State = "Belo Horizonte", Number = "123", Street = "Rua", ZipCode = "1023456" });

            //act
            await _repository.SaveAsync(person);
            var personFound = await _repository.GetAsync(person.Id);

            //assert
            Assert.NotNull(personFound);
        }
    }
}
