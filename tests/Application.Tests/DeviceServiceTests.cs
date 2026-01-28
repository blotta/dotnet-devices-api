using Application.DTOs;
using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace Application.Tests;

public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _repo;
    private readonly DeviceService _service;

    public DeviceServiceTests()
    {
        _repo = new Mock<IDeviceRepository>();
        _service = new DeviceService(_repo.Object);
    }

    [Fact]
    public async Task CreateAsync_creates_device_and_saves()
    {
        Device? device = null;
        _repo.Setup(r => r.AddAsync(It.IsAny<Device>()))
            .Callback<Device>(d => device = d)
            .Returns(Task.CompletedTask);

        var request = new CreateDeviceRequest("Device 1", "Cisco");

        var deviceResponse = await _service.CreateAsync(request);

        device.Should().NotBeNull();
        deviceResponse.Name.Should().Be("Device 1");
        deviceResponse.Brand.Should().Be("Cisco");
        deviceResponse.State.Should().Be(DeviceState.Available);

        _repo.Verify(r => r.AddAsync(It.IsAny<Device>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_should_throw_when_not_found()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Device?)null);

        var act = () => _service.GetByIdAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_should_return_device_when_found()
    {
        var device = new Device("Device 1", "Cisco");

        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(device);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        result.Name.Should().Be("Device 1");
        result.Brand.Should().Be("Cisco");
    }

    [Fact]
    public async Task UpdateAsync_should_update_fields_and_save()
    {
        var device = new Device("Old", "Cisco");

        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(device);

        var request = new UpdateDeviceRequest(
            "New",
             "Juniper",
             null
        );

        var result = await _service.UpdateAsync(Guid.NewGuid(), request);

        result.Name.Should().Be("New");
        result.Brand.Should().Be("Juniper");

        _repo.Verify(r => r.UpdateAsync(device), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_should_throw_if_device_in_use()
    {
        var device = new Device("Device", "Cisco");
        device.ChangeState(DeviceState.InUse);

        _repo
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(device);

        var request = new UpdateDeviceRequest("New", null, null);

        var act = () => _service.UpdateAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<DeviceInUseException>();

        _repo.Verify(r => r.UpdateAsync(It.IsAny<Device>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_should_delete_when_allowed()
    {
        var device = new Device("Device", "Cisco");

        _repo
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(device);

        await _service.DeleteAsync(Guid.NewGuid());

        _repo.Verify(r => r.DeleteAsync(device), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_should_throw_when_device_in_use()
    {
        var device = new Device("Device", "Cisco");
        device.ChangeState(DeviceState.InUse);

        _repo
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(device);

        var act = () => _service.DeleteAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<DeviceInUseException>();

        _repo.Verify(r => r.DeleteAsync(It.IsAny<Device>()), Times.Never);
    }
}
