using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using test.Core.Enums;

namespace test.Core.Entities
{
    public class Office
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public int CityCode { get; set; }

        public string Uuid { get; set; }

        public OfficeType? Type { get; set; }

        public string CountryCode { get; set; } = String.Empty;

        public virtual Coordinate Coordinate { get; set; }

        public Address? Address { get; set; }

        public string WorkTime { get; set; } = String.Empty;

        public virtual Phone Phones { get; set; }

        public int PhonesId { get; set; }

    }
}
