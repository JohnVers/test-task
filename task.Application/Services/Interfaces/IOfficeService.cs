using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.Application.DTOs;
using test.Core.Entities;

namespace task.Application.Services.Interfaces
{
    public interface IOfficeService
    {
        Task<WorkStatsDTO> SaveData(IList<Office> offices, CancellationToken cancellationToken);
    }
}
