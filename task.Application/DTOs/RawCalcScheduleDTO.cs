using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.Application.DTOs
{
    public record RawCalcScheduleDTO
    {
        [JsonProperty("derival")]
        public string Derival { get; set; } = String.Empty;

        [JsonProperty("arrival")]
        public string Arrival { get; set; } = String.Empty;
    }
}
