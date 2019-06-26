using System.Threading.Tasks;
using AspNetCore.AsyncInitialization;
using TestDynamodb.Repositories.Interfaces;

namespace TestDynamodb.Initializers
{
    public class DynamoDBAsyncInitializer : IAsyncInitializer
    {
        private readonly IRegisterTables _registerTables;

        public DynamoDBAsyncInitializer(IRegisterTables registerTables) => _registerTables = registerTables;

        public async Task InitializeAsync() =>  await _registerTables.RegisterAsync();
    }
}
