using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.Application.DTOs
{
    public record RawCitiesListDTO
    {
        [JsonProperty("city")]
        public ICollection<RawCityDTO> Cities { get; set; } = [];
    }
}
