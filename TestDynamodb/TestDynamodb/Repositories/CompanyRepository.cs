using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;
using TestDynamodb.Models;
using TestDynamodb.Repositories.Interfaces;

namespace TestDynamodb.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDynamoDBContext _dBContext;

        public CompanyRepository(IDynamoDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Company> GetAsync(string id)
        {
            return await _dBContext.LoadAsync<Company>(id);
        }

        public async Task SaveAsync(Company company)
        {
            await _dBContext.SaveAsync(company);
        }
    }
}
