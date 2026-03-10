using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.Application.DTOs;

namespace task.Application.Services.Interfaces
{
    public interface IParsingService
    {
        IList<RawCityDTO> ParseData(string filePath);
    }
}
