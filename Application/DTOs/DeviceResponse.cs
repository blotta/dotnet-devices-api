using Domain.Enums;

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
