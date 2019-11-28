using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;
using TestDynamodb.Models;
using TestDynamodb.Repositories.Interfaces;

namespace TestDynamodb.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IDynamoDBContext _dBContext;

        public PersonRepository(IDynamoDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Person> GetAsync(string id)
        {
            return await _dBContext.LoadAsync<Person>(id);
        }

        public async Task SaveAsync(Person person)
        {
            await _dBContext.SaveAsync(person);
        }
    }
}
