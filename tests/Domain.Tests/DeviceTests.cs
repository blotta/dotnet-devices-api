using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests;

public class DeviceTests
{
    [Fact]
    public void Create_should_initialize_device_as_available()
    {
        var name = "Device 1";
        var brand = "Cisco";

        var device = new Device(name, brand);

        device.Name.Should().Be(name);
        device.Brand.Should().Be(brand);
        device.State.Should().Be(DeviceState.Available);
    }

    [Fact]
    public void Rename_should_update_name_when_not_in_use()
    {
        var device = new Device("Device", "Cisco");

        device.Rename("New Name");

        device.Name.Should().Be("New Name");
    }

    [Fact]
    public void Rename_should_throw_when_device_is_in_use()
    {
        var device = new Device("Device", "Cisco");
        device.ChangeState(DeviceState.InUse);

        Action act = () => device.Rename("New Name");

        act.Should().Throw<DeviceInUseException>();
    }

    [Fact]
    public void ChangeBrand_should_update_brand_when_not_in_use()
    {
        var device = new Device("Device", "Cisco");

        device.ChangeBrand("Juniper");

        device.Brand.Should().Be("Juniper");
    }

    [Fact]
    public void ChangeBrand_should_throw_when_device_is_in_use()
    {
        var device = new Device("Device", "Cisco");
        device.ChangeState(DeviceState.InUse);

        Action act = () => device.ChangeBrand("Juniper");

        act.Should().Throw<DeviceInUseException>();
    }

    [Fact]
    public void ChangeState_should_update_state()
    {
        var device = new Device("Device", "Cisco");

        device.ChangeState(DeviceState.Inactive);

        device.State.Should().Be(DeviceState.Inactive);
    }

    [Fact]
    public void EnsureCanBeDeleted_should_not_throw_when_not_in_use()
    {
        var device = new Device("Device", "Cisco");

        Action act = () => device.EnsureCanBeDeleted();

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureCanBeDeleted_should_throw_when_device_is_in_use()
    {
        var device = new Device("Device", "Cisco");
        device.ChangeState(DeviceState.InUse);

        Action act = () => device.EnsureCanBeDeleted();

        act.Should().Throw<DeviceInUseException>();
    }
}
