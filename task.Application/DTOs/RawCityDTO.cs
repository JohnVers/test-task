using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.Application.DTOs
{
    public record RawCityDTO
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("cityID")]
        public int? CityId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("terminals")]
        public RawTerminalListDTO TerminalList { get; set; }
    }
}
