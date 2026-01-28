using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class DeviceInUseException(string message) : DomainException(message)
    {
    }
}
