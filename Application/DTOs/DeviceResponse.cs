using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record DeviceResponse(
        Guid Id,
        string Name,
        string Brand,
        DeviceState State,
        DateTimeOffset CreatedAt
    );
}
