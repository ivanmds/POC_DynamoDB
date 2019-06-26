using System.Threading.Tasks;

namespace TestDynamodb.Repositories.Interfaces
{
    public interface IRegisterTables
    {
        Task RegisterAsync();
    }
}
