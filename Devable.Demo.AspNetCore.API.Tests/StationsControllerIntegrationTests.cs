using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using FluentAssertions;
using System.Net.Http.Headers;
using System.Text;
using System;
using Devable.Demo.AspNetCore.API;
using Devable.Demo.AspNetCore.API.Models.Domain;

namespace WebApiDemo.Tests
{
    public class StationsControllerIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public StationsControllerIntegrationTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task Stations_Get_All()
        {
            // Act
            var response = await _client.GetAsync("/api/Stations");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var stations = JsonConvert.DeserializeObject<IEnumerable<Station>>(responseString);
            stations.Count().Should().Be(50);
        }

        [Fact]
        public async Task Stations_Get_Specific()
        {
            // Act
            var response = await _client.GetAsync("/api/Stations/16");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var station = JsonConvert.DeserializeObject<Station>(responseString);
            station.Id.Should().Be(16);
        }

        [Fact]
        public async Task Stations_Post_Specific()
        {
            // Arrange
            var stationToAdd = new Station
            {
                CallSign = "2DayFM",
                Code = "2DayFM",
                City = "Sydney",
                State = "NSW"
            };
            var content = JsonConvert.SerializeObject(stationToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Stations", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var station = JsonConvert.DeserializeObject<Station>(responseString);
            station.Id.Should().Be(51);
        }

        [Fact]
        public async Task Stations_Post_Specific_Invalid()
        {
            // Arrange
            var stationToAdd = new Station { CallSign = "Bla" };
            var content = JsonConvert.SerializeObject(stationToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Stations", stringContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("The Code field is required")
                .And.Contain("The State field is required");
        }

        [Fact]
        public async Task Stations_Put_Specific()
        {
            // Arrange
            var stationToChange = new Station
            {
                Id = 16,
                CallSign = "2DayFM",
                Code = "2DayFM",
                City = "Sydney",
                State = "NSW"
            };
            var content = JsonConvert.SerializeObject(stationToChange);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/Stations/16", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Be(String.Empty);
        }


        [Fact]
        public async Task Stations_Put_Specific_Invalid()
        {
            // Arrange
            var stationToChange = new Station { CallSign = "John" };
            var content = JsonConvert.SerializeObject(stationToChange);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/Stations/16", stringContent);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("The Code field is required")
                .And.Contain("The State field is required");
        }

        [Fact]
        public async Task Stations_Delete_Specific()
        {
            // Arrange

            // Act
            var response = await _client.DeleteAsync("/api/Stations/16");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Be(String.Empty);
        }
    }
}