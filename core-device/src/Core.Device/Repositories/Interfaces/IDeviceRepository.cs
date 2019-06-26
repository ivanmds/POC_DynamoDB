using Core.Device.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Device.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<List<DeviceData>> GetAll(int quantity, CancellationToken cancellationToken = default);
        Task<List<DeviceData>> ListAsync(Expression<Func<DeviceData, bool>> query, CancellationToken cancellationToken = default);
        Task<DeviceData> SaveAsync(DeviceData device);
        Task<DeviceData> SelectAsync(Expression<Func<DeviceData, bool>> query, CancellationToken cancellationToken = default);
        Task<string> GetPushTokenAsync(int customerId, CancellationToken cancellationToken);
        Task RegisterTablesAsync();
    }
}
