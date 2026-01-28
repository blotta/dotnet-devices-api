using Domain.Entities;
using Domain.Queries;

namespace Application.Repositories
{
    public interface IDeviceRepository
    {
        Task AddAsync(Device device);
        Task DeleteAsync(Device device);
        Task<List<Device>> GetAsync(DeviceFilter filter);
        Task<Device?> GetByIdAsync(Guid id);
        Task UpdateAsync(Device device);
    }
}
