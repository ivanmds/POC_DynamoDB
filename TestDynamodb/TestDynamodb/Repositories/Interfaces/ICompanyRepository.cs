using System.Threading.Tasks;
using TestDynamodb.Models;

namespace TestDynamodb.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task SaveAsync(Company company);
        Task<Company> GetAsync(string id);
    }
}
