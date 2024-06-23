using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Logging;
using StargateAPI.Controllers;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Commands;

namespace apiTests
{
    public class PersonControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<PersonController>> _loggerMock;
        private readonly PersonController _controller;

        public PersonControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PersonController>>();
            _controller = new PersonController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetPeople_ReturnsOkResult_WhenPeopleExist()
        {
            // Arrange
            var people = new List<StargateAPI.Business.Dtos.PersonAstronaut> { new StargateAPI.Business.Dtos.PersonAstronaut { /* initialize properties here */ } };
            var getPeopleResult = new GetPeopleResult { People = people }; // replace with the actual way to create a GetPeopleResult
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPeople>(), default)).ReturnsAsync(getPeopleResult);

            // Act
            var result = await _controller.GetPeople();

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnValue = Assert.IsType<GetPeopleResult>(okResult.Value);
            Assert.Equal(getPeopleResult, returnValue);
        }

        // create test for GetPeople when an exception is thrown
        [Fact]
        public async Task GetPeople_ReturnsInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPeople>(), default)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetPeople();

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
        }

        // create test for GetPersonByName
        [Fact]
        public async Task GetPersonByName_ReturnsOkResult_WhenPersonExists()
        {
            // Arrange
            var person = new StargateAPI.Business.Dtos.PersonAstronaut { /* initialize properties here */ };
            var getPersonByNameResult = new GetPersonByNameResult { Person = person }; // replace with the actual way to create a GetPersonByNameResult
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonByName>(), default)).ReturnsAsync(getPersonByNameResult);

            // Act
            var result = await _controller.GetPersonByName("Test Name");

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnValue = Assert.IsType<GetPersonByNameResult>(okResult.Value);
            Assert.Equal(getPersonByNameResult, returnValue);
        }

        [Fact]
        public async Task GetPersonByName_ReturnsInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonByName>(), default)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetPersonByName("Test Name");

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
        }

        // add person creation test
        [Fact]
        public async Task CreatePerson_ReturnsOkResult_WhenPersonIsCreated()
        {
            // Arrange
            var person = new StargateAPI.Business.Dtos.PersonAstronaut { Name = "Jeff Doe" };
            var createPersonResult = new CreatePersonResult { };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePerson>(), default)).ReturnsAsync(createPersonResult);

            // Act
            var result = await _controller.CreatePerson(person.Name);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnValue = Assert.IsType<CreatePersonResult>(okResult.Value);
            Assert.Equal(createPersonResult, returnValue);
        }

        // add person creation test when an exception is thrown
        [Fact]
        public async Task CreatePerson_ReturnsInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            var person = new StargateAPI.Business.Dtos.PersonAstronaut { Name = "Jeff Doe" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePerson>(), default)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreatePerson(person.Name);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
        }
    }
}