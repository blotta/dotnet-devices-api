using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record UpdateDeviceRequest(
        string? Name,
        string? Brand,
        DeviceState? State
    );
}
