using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Core.Entities
{
    public record Address(string? Region, string? City, string? Street, string? HouseNumber, int? Apartment) { }
}
