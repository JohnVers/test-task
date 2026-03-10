using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.Application.DTOs;
using test.Core.Entities;

namespace task.Application.Services.Interfaces
{
    public interface IMappingService
    {
        IList<Office> MapRawData(IList<RawCityDTO> data);
    }
}
