using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestDynamodb.Repositories.Interfaces
{
    public interface IEventRepository : IDisposable
    {
        Task<IEnumerable<object>> ListEventsAsync(string account);
    }
}
