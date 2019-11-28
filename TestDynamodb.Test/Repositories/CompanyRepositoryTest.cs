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
    public class CompanyRepositoryTest
    {
        private ICompanyRepository _repository;

        public CompanyRepositoryTest()
        {
            var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
            var dynamoDB = new AmazonDynamoDBClient("root", "secret", clientConfig);
            var context = new DynamoDBContext(dynamoDB);
            _repository = new CompanyRepository(context);
        }

        [Fact]
        public async Task SaveNewPerson()
        {
            //arrange
            var company = new Company();
            company.Id = Guid.NewGuid().ToString();

            var partner = new Partner();
            partner.Name = "Teste";

            partner.Phones.Add(new Phone { DDD = "11", Number = "123456789" });
            partner.Phones.Add(new Phone { DDD = "11", Number = "987654321" });

            partner.Addresses.Add(new Address { City = "SP", State = "São Paulo", Number = "123", Street = "Rua", ZipCode = "1023456" });
            partner.Addresses.Add(new Address { City = "BH", State = "Belo Horizonte", Number = "123", Street = "Rua", ZipCode = "1023456" });

            company.Partners.Add(partner);

            company.Phones.Add(new Phone { DDD = "11", Number = "123456789" });
            company.Phones.Add(new Phone { DDD = "11", Number = "987654321" });

            company.Addresses.Add(new Address { City = "SP", State = "São Paulo", Number = "123", Street = "Rua", ZipCode = "1023456" });
            company.Addresses.Add(new Address { City = "BH", State = "Belo Horizonte", Number = "123", Street = "Rua", ZipCode = "1023456" });


            //act
            await _repository.SaveAsync(company);
            var companyFound = await _repository.GetAsync(company.Id);

            Company companyUpdated = null;

            if (companyFound != null)
            {
                companyFound.Name = "Teste 2";
                companyFound.Phones.Add(new Phone { DDD = "11", Number = "123456789" });

                companyFound.Addresses.Add(new Address { City = "SP", State = "São Paulo", Number = "123", Street = "Rua", ZipCode = "1023456" });

                companyFound.Partners.Clear();

                await _repository.SaveAsync(companyFound);
                companyUpdated = await _repository.GetAsync(company.Id);
            }

            //assert
            Assert.NotNull(companyFound);
            Assert.NotNull(companyUpdated);
            Assert.Equal(3, companyUpdated.Phones.Count);
            Assert.Equal(3, companyUpdated.Addresses.Count);
            Assert.Equal(0, companyUpdated.Partners.Count);
        }
    }
}
