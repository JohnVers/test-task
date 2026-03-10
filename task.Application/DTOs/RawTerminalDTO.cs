using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace task.Application.DTOs
{
    public record RawTerminalDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; } = String.Empty;

        [JsonProperty("isPVZ")]
        public bool IsPVZ { get; set; }


        [JsonProperty("storage")]
        public bool IsStorage { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; } = String.Empty;

        [JsonProperty("longitude")]
        public string Longitude { get; set; } = String.Empty;

        [JsonProperty("address")]
        public string Address { get; set; } = String.Empty;

        [JsonProperty("fullAddress")]
        public string FullAddress { get; set; } = String.Empty;

        [JsonProperty("phones")]
        public ICollection<RawPhoneDTO> Phones { get; set; } = [];

        [JsonProperty("mainPhone")]
        public string MainPhone { get; set; } = String.Empty;

        [JsonProperty("calcSchedule")]
        public RawCalcScheduleDTO CalcSchedule { get; set; }

    }
}
