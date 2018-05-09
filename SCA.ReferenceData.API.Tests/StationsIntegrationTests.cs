using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Devable.Demo.AspNetCore.API.Tests
{
    public class StationsIntegrationTests
    {
        private readonly HttpClient _client;

        public StationsIntegrationTests()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://referenceapi-test.scalabs.com.au");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task GetStations_WhenHitNetworkAndRequestStations_ThenReturnsStatusOK()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/hit/stations");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            //var stations = JsonConvert.DeserializeObject<IEnumerable<Stations>>(responseString);
            //stations.Count().Should().Be(25);

            responseString.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetStations_WhenHitNetworkAndRequestStationsBySydneyPostcode_ThenReturnsStatusOK()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/hit/stations?IsNearestSearch=true&Postcode=2000");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            //var stations = JsonConvert.DeserializeObject<IEnumerable<Stations>>(responseString);
            //stations.Count().Should().Be(25);

            responseString.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public async Task GetStations_WithInvalidParams_ThenReturnsStatusBadRequest()
        {
            // Act
            string result = string.Empty;
            HttpResponseMessage response = null;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    response = await _client.GetAsync("/api/v1/hit/stations?IsNearestSearch=X&Postcode=-1&anotherparam=x");
                    result = response.Content.ReadAsStringAsync().Result;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            // Assert
            //var stations = JsonConvert.DeserializeObject<IEnumerable<Stations>>(responseString);
            //stations.Count().Should().Be(25);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNullOrEmpty();
        }
    }
}
