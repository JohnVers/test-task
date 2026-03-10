using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.Application.DTOs
{
    public record RawTerminalListDTO
    {
        [JsonProperty("terminal")]
        public ICollection<RawTerminalDTO> Terminals { get; set; } = []; 
    }
}
