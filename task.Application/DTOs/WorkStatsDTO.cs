using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.Application.DTOs
{
    public record WorkStatsDTO(int TotalTerminals, int DeletedCount, int SavedCount, string? ErrorMessage);
    
}
