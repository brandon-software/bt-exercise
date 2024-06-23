using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Dtos;

namespace apiTests
{
    public class GetPersonByNameResultTest
    {
        public PersonAstronaut? person { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public int responseCode { get; set; }
    }
    public class EndpointTests
    {
        private readonly HttpClient _client;

        public EndpointTests()
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
            Assert.NotNull(peopleResponse.People);
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

        // test create person
        [Fact]
        async Task CreatePerson_ReturnsOkResult_WhenPersonIsCreated()
        {
            // Arrange
            // Arrange
            var personName = "Test Person 1";

            // Act
            var response = await _client.PostAsJsonAsync("/Person", personName);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var createPersonResponse = JsonConvert.DeserializeObject<CreatePersonResult>(responseString);
            Assert.True(createPersonResponse.Success);
            Assert.NotNull(createPersonResponse.Id);
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

        // test post astronaut duty
        [Fact]
        async Task PostAstronautDuty_ReturnsOkResult_WhenDutyIsCreated()
        {
            // Arrange
            var astronautDuty = new
            {
                name = "John Doe",
                rank = "captain",
                dutyTitle = "mr",
                dutyStartDate = "2024-06-21T19:19:49.988Z",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/AstronautDuty", astronautDuty);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var createAstronautDutyResponse = JsonConvert.DeserializeObject<CreateAstronautDutyResult>(responseString);
            Assert.True(createAstronautDutyResponse.Success);
            Assert.NotNull(createAstronautDutyResponse.Id);
        }
    }
}