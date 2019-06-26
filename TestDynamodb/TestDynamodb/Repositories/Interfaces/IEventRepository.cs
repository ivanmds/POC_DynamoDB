using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestDynamodb.Models;

namespace TestDynamodb.Repositories.Interfaces
{
    public interface IEventRepository : IDisposable
    {
        Task<IEnumerable<Event>> ListEventsAsync(string account);
    }
}
