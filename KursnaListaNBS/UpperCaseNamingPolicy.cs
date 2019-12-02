using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace KursnaListaNBS
{
    public class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name switch
            {
                null => throw new ArgumentNullException(nameof(name)),
                "" => throw new ArgumentException($"{nameof(name)} cannot be empty", nameof(name)),
                _ => name.First().ToString().ToUpper() + name.Substring(1)
            };
    }
}
