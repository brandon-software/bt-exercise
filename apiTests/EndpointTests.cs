using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using StargateAPI.Business.Commands;

namespace apiTests
{
    public class PeopleResponse
    {
        public List<StargateAPI.Business.Dtos.PersonAstronaut>? People { get; set; }
    }

    public class PersonResponse
    {
        public StargateAPI.Business.Dtos.PersonAstronaut? Person { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ResponseCode { get; set; }
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
            var peopleResponse = JsonConvert.DeserializeObject<PeopleResponse>(responseString);
            Assert.NotEmpty(peopleResponse.People);
        }

        [Fact]
        async Task GetPersonByName_ReturnsOkResult_WhenPersonExists()
        {
            // Arrange
            var personName = "brandon";

            // Act
            var response = await _client.GetAsync($"/Person/{personName}");

            // Assert
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var personResponse = JsonConvert.DeserializeObject<PersonResponse>(responseString);
                var person = personResponse.Person;
                Assert.Equal(personName, person.Name);
            }
        }
        [Fact]
        async Task GetPersonByName_ReturnsNotFoundResult_WhenPersonDoesNotExist()
        {
            // Arrange
            var personName = "nonexistent";

            // Act
            var response = await _client.GetAsync($"/Person/{personName}");

            // Assert
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var personResponse = JsonConvert.DeserializeObject<PersonResponse>(responseString);
                Assert.Null(personResponse.Person);
            }
        }
    }
}