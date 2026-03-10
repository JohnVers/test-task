using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.Application.DTOs;
using task.Application.Services.Implementation;

namespace task.Tests
{
    public class MappingTests
    {

        private Mock<ILogger<MappingService>> _loggerMock;
        private MappingService _service;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<MappingService>>();
            _service = new MappingService(_loggerMock.Object);
        }


        [Test]
        public void MapAddress_ReturnTrue()
        {
            var rawData = BuildTerminalsData("414057, Астраханская обл, Астрахань г, 1-й проезд Рождественского ул, стр 5");

            var offices = _service.MapRawData(rawData);

            Assert.That(offices, !Is.Null);

            Assert.That(offices.Count, Is.EqualTo(1));

            var office = offices.First();

            Assert.That(office.Address?.City, Is.EqualTo("Астрахань г"));

            Assert.That(office.Address?.Region, Is.EqualTo("414057, Астраханская обл"));

            Assert.That(office.Address?.Street, Is.EqualTo("1-й проезд Рождественского ул"));

            Assert.That(office.Address?.HouseNumber, Is.EqualTo("стр 5"));
        }


        [Test]
        public void MapAddress2_ReturnTrue()
        {
            var rawData = BuildTerminalsData("117545, Москва г, Подольских Курсантов ул, дом № 17, корпус 2");

            var offices = _service.MapRawData(rawData);

            Assert.That(offices, !Is.Null);

            Assert.That(offices.Count, Is.EqualTo(1));

            var office = offices.First();

            Assert.That(office.Address?.City, Is.EqualTo("Москва г"));

            Assert.That(office.Address?.Region, Is.EqualTo("117545"));

            Assert.That(office.Address?.Street, Is.EqualTo("Подольских Курсантов ул"));

            Assert.That(office.Address?.HouseNumber, Is.EqualTo("дом № 17, корпус 2"));
        }

        private IList<RawCityDTO> BuildTerminalsData(string fullAddress)
        {
            return new List<RawCityDTO>()
            {
                new RawCityDTO()
                {
                    Code = "Code",
                    CityId = 1,
                    Name = "Test City",
                    TerminalList = new RawTerminalListDTO()
                    {
                        Terminals = [
                            new RawTerminalDTO()
                            {
                                FullAddress = fullAddress,
                                CalcSchedule = new RawCalcScheduleDTO()
                                {
                                    Arrival = "arrival",
                                    Derival = "derival"
                                },
                                IsPVZ = true,
                                Latitude = "123.3",
                                Longitude = "123.4",
                                MainPhone = "7 912 312 123",
                                Name = "Test Name",
                                Phones = []
                            }
                        ]
                    },

                }
            };
        }
    }
}
