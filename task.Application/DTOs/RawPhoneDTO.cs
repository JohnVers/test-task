using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.Application.DTOs
{
    public record RawPhoneDTO
    {
        [JsonProperty("number")]
        public string Number { get; set; } = String.Empty;

        [JsonProperty("primary")]
        public bool IsPrimary { get; set; }
    }
}
