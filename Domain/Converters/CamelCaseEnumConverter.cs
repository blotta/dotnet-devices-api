using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Converters
{
    public class CamelCaseEnumConverter: JsonStringEnumConverter
    {
        public CamelCaseEnumConverter()
            : base(JsonNamingPolicy.CamelCase)
        { }
    }
}
