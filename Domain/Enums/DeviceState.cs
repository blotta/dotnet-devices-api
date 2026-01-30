using Domain.Converters;
using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(CamelCaseEnumConverter))]
    public enum DeviceState
    {
        Available = 0,
        InUse = 1,
        Inactive = 2
    }
}
