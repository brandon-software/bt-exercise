using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using StargateAPI.Controllers;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using StargateAPI.Business.Commands;

namespace apiTests
{
    public class AstronautDutyControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AstronautDutyController _controller;

        public AstronautDutyControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AstronautDutyController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateAstronautDuty_ReturnsOkResult_WhenDutyIsCreated()
        {
            // Arrange
            var request = new CreateAstronautDuty 
            { 
                Name = "Test Name",
                Rank = "Test Rank",
                DutyTitle = "Test DutyTitle",
                DutyStartDate = new System.DateTime(2021, 1, 1)                
            };
            var result = new CreateAstronautDutyResult { Id = 1 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateAstronautDuty>(), default)).ReturnsAsync(result);

            // Act
            var response = await _controller.CreateAstronautDuty(request);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(response);
            var returnValue = Assert.IsType<CreateAstronautDutyResult>(okResult.Value);
            Assert.Equal(result.Id, returnValue.Id);
        }
        // get all duties by name does not exist
        [Fact]
        public async Task GetAstronautDutiesByName_ReturnsNotFoundResult_WhenDutiesDoNotExist()
        {
            // Arrange
            var duties = new List<AstronautDuty>();
            var getDutiesResult = new GetAstronautDutiesByNameResult { AstronautDuties = duties };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAstronautDutiesByName>(), default)).ReturnsAsync(getDutiesResult);
            
            // Act
            var result = await _controller.GetAstronautDutiesByName("Test Name");
            
            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnValue = Assert.IsType<BaseResponse>(okResult.Value);
            Assert.False(returnValue.Success);
        }
        
    }
}