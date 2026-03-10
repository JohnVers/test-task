using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Core.Entities
{
    public class Phone
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; } = String.Empty;

        public string? Additional { get; set; }

        public virtual Office Office { get; set; }

        public int OfficeId { get; set; }
    }
}
