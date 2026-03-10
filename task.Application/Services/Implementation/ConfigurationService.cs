using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.Application.Services.Interfaces;

namespace task.Application.Services.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _configuration;

        private const string FILE_LOCATION_KEY = "AppSettings:FileLocation";

        private const string EXEUTION_TIME_KEY = "AppSettings:ExecutionTime";

        public ConfigurationService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string GetFileLocation()
        {
            var result = GetStringValue(FILE_LOCATION_KEY);

            return result;
        }

        public string? GetExecutionTime()
        {
            var result = GetStringValue(EXEUTION_TIME_KEY, false);

            return result;
        }

        private string? GetStringValue(string key, bool IsRequired = true)
        {
            var value = _configuration[key];

            if (String.IsNullOrEmpty(value) && IsRequired)
                throw new Exception($"Key not found: {key}");

            return value;
        }

    }
}
