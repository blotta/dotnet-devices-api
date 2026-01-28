using Domain.Enums;

namespace Domain.Queries
{
    public record DeviceFilter(string? Brand, DeviceState? State);
}
