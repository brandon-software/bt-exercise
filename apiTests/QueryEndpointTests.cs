using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Dtos;


namespace apiTests.Query
{
    public class GetPersonByNameResultTest
    {
        public PersonAstronaut? person { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
        public int responseCode { get; set; }
    }

    public class QueryEndpointTests
    {
        private readonly HttpClient _client;
        public QueryEndpointTests()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _client = new HttpClient(handler);
            _client.BaseAddress = new System.Uri("http://localhost:5204"); // replace with API's base URL

        }

        [Fact]
        async Task GetPeople_ReturnsOkResult_WhenPeopleExist()
        {
            // Act
            var response = await _client.GetAsync("/Person");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var peopleResponse = JsonConvert.DeserializeObject<GetPeopleResult>(responseString);
            Assert.NotNull(peopleResponse?.People);
        }

        [Fact]
        async Task GetPersonByName_ReturnsNotFoundResult_WhenPersonDoesNotExist()
        {
            // Arrange
            var personName = "non existent person";

            // Act
            var response = await _client.GetAsync($"/Person/{personName}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var personResponse = JsonConvert.DeserializeObject<GetPersonByNameResultTest>(responseString);
            Assert.Null(personResponse?.person);
        }

        // test get person by name
        [Fact]
        async Task GetPersonByName_ReturnsOkResult_WhenPersonExists()
        {
            // Arrange
            var personName = "John Doe";

            // Act
            var response = await _client.GetAsync($"/Person/{personName}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var personResponse = JsonConvert.DeserializeObject<GetPersonByNameResultTest>(responseString);
            Assert.NotNull(personResponse?.person);
        }

        // test get astronaut duties by name
        [Fact]
        async Task GetAstronautDutiesByName_ReturnsOkResult_WhenPersonExists()
        {
            // Arrange
            var personName = "John Doe";

            // Act
            var response = await _client.GetAsync($"/AstronautDuty/{personName}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var astronautDutiesResponse = JsonConvert.DeserializeObject<GetAstronautDutiesByNameResult>(responseString);
            Assert.NotNull(astronautDutiesResponse?.Person);
            Assert.NotNull(astronautDutiesResponse?.AstronautDuties);
        }
    }
}