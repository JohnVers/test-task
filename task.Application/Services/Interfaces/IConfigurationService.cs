using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task.Application.Services.Interfaces
{
    public interface IConfigurationService
    {
        string GetFileLocation();

        string? GetExecutionTime();
    }
}
