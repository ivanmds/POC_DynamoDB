using System.Threading.Tasks;
using TestDynamodb.Models;

namespace TestDynamodb.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task SaveAsync(Person person);
        Task<Person> GetAsync(string id);
    }
}
