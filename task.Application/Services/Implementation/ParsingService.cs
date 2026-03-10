using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.Application.DTOs;
using task.Application.Services.Interfaces;

namespace task.Application.Services.Implementation
{
    public class ParsingService : IParsingService
    {
        private readonly ILogger<ParsingService> _logger;

        public ParsingService(ILogger<ParsingService> logger)
        {
            _logger = logger;
        }

        public IList<RawCityDTO> ParseData(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                
                var data = JsonConvert.DeserializeObject<RawCitiesListDTO>(jsonString);

                return data.Cities.ToList();
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"The file '{filePath}' was not found.");

                throw ex;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error deserializing JSON: {ex.Message}");

                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                throw ex;
            }
        }
    }
}
