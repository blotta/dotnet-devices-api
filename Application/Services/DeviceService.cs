using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using Domain.Entities;
using Domain.Queries;

namespace Application.Services
{
    public class DeviceService(IDeviceRepository _repository)
    {
        public async Task<DeviceResponse> CreateAsync(CreateDeviceRequest request)
        {
            var device = new Device(request.Name, request.Brand);

            await _repository.AddAsync(device);

            return ToResponse(device);
        }

        public async Task<DeviceResponse> GetByIdAsync(Guid id)
        {
            var device = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException("Device not found");

            return ToResponse(device);
        }

        public async Task<List<DeviceResponse>> GetAsync(DeviceFilter filter)
        {
            var devices = await _repository.GetAsync(filter);
            return [.. devices.Select(ToResponse)];
        }

        public async Task<DeviceResponse> UpdateAsync(
            Guid id,
            UpdateDeviceRequest request
        )
        {
            var device = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException("Device not found");

            // change state first, dependent operatins applied later
            if (request.State.HasValue)
                device.ChangeState(request.State.Value);

            if (!string.IsNullOrWhiteSpace(request.Name))
                device.Rename(request.Name);

            if (!string.IsNullOrWhiteSpace(request.Brand))
                device.ChangeBrand(request.Brand);

            await _repository.UpdateAsync(device);

            return ToResponse(device);
        }

        public async Task DeleteAsync(Guid id)
        {
            var device = await _repository.GetByIdAsync(id)
                ?? throw new NotFoundException("Device not found");

            device.EnsureCanBeDeleted();

            await _repository.DeleteAsync(device);
        }
        private static DeviceResponse ToResponse(Device device)
        {
            return new DeviceResponse(
                device.Id,
                device.Name,
                device.Brand,
                device.State,
                device.CreatedAt
            );
        }
    }
}
