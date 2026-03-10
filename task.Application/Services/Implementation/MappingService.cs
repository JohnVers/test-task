using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using task.Application.DTOs;
using task.Application.Services.Interfaces;
using test.Core.Entities;
using test.Core.Enums;

namespace task.Application.Services.Implementation
{
    public class MappingService : IMappingService
    {
        private const string DEFAULT_COUNTRY_CODE = "RU";

        private const string CITY_REGEX_PATTERN = @"([А-Я][а-я,-]+)+ (г|с|пгт|д|п|рп).?";

        private const string STREET_REGEX_PATTERN = @"\s(ул|пер|ш|проезд|пр-кт|км|б-р|ул|тер|тракт|дор|кв-л|промузел|]).?$";

        private const string HOUSE_REGEX_PATTERN = @"^\s(дом|[Л|л]итера|стр|корпус|з\/у|владение)";

        private const string APPARTMENTY_REGEX_PATTERN = @"\d+";

        private const char ADDRESS_PARTS_DELIMITER = ',';

        private readonly ILogger<MappingService> _logger;

        public MappingService(ILogger<MappingService> logger)
        {
            _logger = logger;
        }

        public IList<Office> MapRawData(IList<RawCityDTO> data)
        {
            var result = new ConcurrentBag<Office>();

            Parallel.ForEach(data, (item, state, index) =>
            {
                foreach (var terminal in item.TerminalList.Terminals)
                {
                    var office = new Office()
                    {
                        Code = item.Code,
                        CityCode = MapCityId(item),
                        Type = MapType(terminal),
                        CountryCode = DEFAULT_COUNTRY_CODE,
                        Coordinate = new Coordinate(ParseCoordinate(terminal.Latitude), ParseCoordinate(terminal.Longitude)),
                        Address = MapAddress(terminal),
                        Phones = MapPhone(terminal),
                        WorkTime = terminal.CalcSchedule.Arrival,
                        Uuid = terminal.Name
                    };

                    result.Add(office);
                }
            });

            return result.ToList();
        }

        private OfficeType MapType(RawTerminalDTO terminal)
        {
            return terminal.IsPVZ ? OfficeType.PVZ :
                terminal.IsStorage ? OfficeType.WAREHOUSE : OfficeType.POSTAMAT;
        }

        private double ParseCoordinate(string val)
        {
            if (!double.TryParse(val, out var result))
                throw new Exception($"Can't parse double value: {val}");

            return result;
        }

        private int MapCityId(RawCityDTO city)
        {
            if (!city.CityId.HasValue)
                throw new Exception($"CityId is null for {city.Name}");

            return city.CityId.Value;
        }

        private Address MapAddress(RawTerminalDTO terminal)
        {
            string? region = String.Empty;
            string city = String.Empty;
            string street = String.Empty;
            string houseNumber = String.Empty;
            int? appartment = null;

            var addressStr = terminal.FullAddress;

            var addressParts = addressStr.Split(ADDRESS_PARTS_DELIMITER);

            Match cityMatch = Regex.Match(addressStr, CITY_REGEX_PATTERN);

            if (cityMatch.Success)
            {
                city = cityMatch.Groups[0].Value
                    .TrimEnd(ADDRESS_PARTS_DELIMITER);
            }
            else
            {
                _logger.LogError($"Can't parse Город: {addressStr}");
            }

            var regionStr = addressStr.Substring(0, cityMatch.Index);

            region = regionStr
                .TrimEnd()
                .TrimEnd(ADDRESS_PARTS_DELIMITER);

            if (String.IsNullOrEmpty(region))
                _logger.LogError($"Can't parse Регион: {addressStr}");

            var houseParts = new List<string>();

            foreach (var part in addressParts)
            {
                var streetMatch = Regex.Match(part, STREET_REGEX_PATTERN);

                var houseMatch = Regex.Match(part, HOUSE_REGEX_PATTERN);

                if (streetMatch.Success)
                {
                    street = part.Trim();
                }

                if (houseMatch.Success)
                {
                    houseParts.Add(part.Trim());
                }

            }
            
            if (String.IsNullOrEmpty(street))
            {
                _logger.LogError($"Can't parse Улица: {addressStr}");
            }

            houseNumber = String.Join(", ", houseParts);

            if (String.IsNullOrEmpty(houseNumber))
            {
                _logger.LogError($"Can't parse Номер Дома: {addressStr}");
            }
            else
            {
                var appartmentStr = addressStr.Substring(addressStr.IndexOf(houseNumber) + houseNumber.Length);

                if (!String.IsNullOrEmpty(appartmentStr))
                {

                    Match appartmentMatch = Regex.Match(appartmentStr, APPARTMENTY_REGEX_PATTERN);

                    if (appartmentMatch.Success)
                    {
                        var value = appartmentMatch.Groups[0].Value
                            .TrimEnd();

                        appartment = Int32.Parse(value);
                    }
                }
            }

            if (!appartment.HasValue)
            {
                _logger.LogError($"Can't parse Appartment: {addressStr}");
            }

            

            var result = new Address(region, city, street, houseNumber, appartment);

            return result;
        }

        private Phone MapPhone(RawTerminalDTO terminal)
        {
            return new Phone()
            {
                PhoneNumber = terminal.MainPhone,
                Additional = terminal?.Phones.FirstOrDefault(p => !p.IsPrimary)?.Number
            };
        }
    }
}
