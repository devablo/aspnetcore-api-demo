using Devable.Demo.AspNetCore.API.Controllers;
using Devable.Demo.AspNetCore.API.Models.Domain;
using Devable.Demo.AspNetCore.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Devable.Demo.AspNetCore.API.Tests
{
    public class StationsControllerUnitTests
    {
        [Fact]
        public async Task Stations_Get_All()
        {
            // Arrange
            var controller = new StationsController(new StationService());

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var stations = okResult.Value.Should().BeAssignableTo<IEnumerable<Station>>().Subject;

            stations.Count().Should().Be(50);
        }

        [Fact]
        public async Task Stations_Get_Specific()
        {
            // Arrange
            var controller = new StationsController(new StationService());

            // Act
            var result = await controller.Get(16);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var station = okResult.Value.Should().BeAssignableTo<Station>().Subject;
            station.Id.Should().Be(16);
        }

        [Fact]
        public async Task Stations_Add()
        {
            // Arrange
            var controller = new StationsController(new StationService());
            var nowObject = new Station
            {
                CallSign = "2DayFM",
                Code = "2DayFM",
                City = "Sydney",
                State = "NSW"
            };

            // Act
            var result = await controller.Post(nowObject);

            // Assert
            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var station = okResult.Value.Should().BeAssignableTo<Station>().Subject;
            station.Id.Should().Be(51);
        }

        [Fact]
        public async Task Stations_Change()
        {
            // Arrange
            var service = new StationService();
            var controller = new StationsController(service);
            var nowObject = new Station
            {
                CallSign = "2DayFM",
                Code = "2DayFM",
                City = "Sydney",
                State = "NSW"
            };

            // Act
            var result = await controller.Put(20, nowObject);

            // Assert
            var okResult = result.Should().BeOfType<NoContentResult>().Subject;

            var station = service.Get(20);
            station.Id.Should().Be(20);
            station.CallSign.Should().Be("2DayFM");
            station.Code.Should().Be("2DayFM");
            station.City.Should().Be("Sydney");
            station.State.Should().Be("NSW");
        }

        [Fact]
        public async Task Stations_Delete()
        {
            // Arrange
            var service = new StationService();
            var controller = new StationsController(service);

            // Act
            var result = await controller.Delete(20);

            // Assert
            var okResult = result.Should().BeOfType<NoContentResult>().Subject;

            // should throw an eception, 
            // because the station with id==20 doesn't exist enymore
            AssertionExtensions.ShouldThrow<InvalidOperationException>(
              () => service.Get(20));
        }
    }
}
