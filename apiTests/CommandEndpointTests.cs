using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Dtos;
using Xunit.Sdk;
using Xunit.Abstractions;

namespace apiTests
{
    public class GetPersonByNameResultTest
    {
        public PersonAstronaut? Person { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int ResponseCode { get; set; }
    }

    public static class TestContext
    {
        private static string _personName = string.Empty;
        private static string _dutyStartDate = string.Empty;
        public static string PersonName
        {
            get { return _personName; }
            set { if (string.IsNullOrEmpty(_personName)) _personName = value; }
        }
        public static string DutyStartDate
        {
            get { return _dutyStartDate; }
            set { if (string.IsNullOrEmpty(_dutyStartDate)) _dutyStartDate = value; }
        }

    }

    public class CommandEndpointTests
    {
        private readonly HttpClient _client;

        public CommandEndpointTests()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _client = new HttpClient(handler);
            _client.BaseAddress = new System.Uri("http://localhost:5204"); // replace with API's base URL

            var randomNames = new List<string>
            {
                "John Smith",
                "Jane Doe",
                "Emily Davis",
                "Daniel Wilson",
                "Olivia Martinez",
                "David Anderson",
                "Sophia Taylor",
                "Joseph Thomas",
                "Emma Hernandez",
                "Matthew Moore",
                "Ava Clark",
                "Jane Smith",
                "David Brown"
            };
            var random = new Random();
            TestContext.PersonName = randomNames[random.Next(randomNames.Count)] + new Random().Next(1, 1001);
            // create array dates in last 12 years
            var dates = new List<DateTime>();
            for (int i = 0; i < 12; i++)
            {
                dates.Add(DateTime.Now.AddYears(-i));
            }
            random = new Random();
            TestContext.DutyStartDate = dates[random.Next(dates.Count)].ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        // test create person

        [Fact, Priority(1)]
        public async Task CreatePerson_ReturnsOkResult_WhenPersonIsCreated()
        {
            // Arrange
            var personName = TestContext.PersonName;
            Console.WriteLine(personName);

            // Act
            var response = await _client.PostAsJsonAsync("/Person", personName);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var createPersonResponse = JsonConvert.DeserializeObject<CreatePersonResult>(responseString);
            Assert.True(createPersonResponse?.Success);
            Assert.NotNull(createPersonResponse?.Id);
        }

        // test post astronaut duty depends on create person
        [Fact, Priority(2)]
        public async Task PostAstronautDuty_ReturnsOkResult_WhenDutyIsCreated()
        {
            // Arrange
            var astronautDuty = new
            {
                name = TestContext.PersonName,
                rank = "2LT",
                dutyTitle = "Commander",
                dutyStartDate = TestContext.DutyStartDate
            };
            Console.WriteLine(astronautDuty);

            // Act
            var response = await _client.PostAsJsonAsync("/AstronautDuty", astronautDuty);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var createAstronautDutyResponse = JsonConvert.DeserializeObject<CreateAstronautDutyResult>(responseString);
            Assert.True(createAstronautDutyResponse?.Success);
            Assert.NotNull(createAstronautDutyResponse?.Id);
        }
    }
}