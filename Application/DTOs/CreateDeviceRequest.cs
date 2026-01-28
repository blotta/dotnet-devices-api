using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record CreateDeviceRequest(
        string Name,
        string Brand
    );
}
