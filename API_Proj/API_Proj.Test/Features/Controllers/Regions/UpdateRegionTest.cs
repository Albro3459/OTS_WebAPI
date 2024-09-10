using Microsoft.AspNetCore.Mvc;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;
using API_Proj.Features.Request.Regions;
using API_Proj.Domain.Entity;

namespace API_Proj.Test.Features.Controllers.Regions;

[TestClass]
public class UpdateRegionTest {

    private RegionsController _regionsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public UpdateRegionTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _regionsController = new RegionsController(_mediatorMock.Object);

    }

    // UPDATE Tests:
    [TestMethod]
    public async Task UpdateRegion_Valid_ShouldReturnOk() {
        //Arrange
        var expectedResultDTO = _fixture.Create<RegionDTO>();
        var expectedResult = new OkObjectResult(expectedResultDTO);
        var dtoToUpdate = _fixture.Create<RegionDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateRegion.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _regionsController.UpdateRegion(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var okResult = DTOResult as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult.Value, okResult.Value);
    }

    [TestMethod]
    public async Task UpdateRegion_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResult = new BadRequestObjectResult("Region can't be null");
        var dtoToUpdate = _fixture.Create<RegionDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateRegion.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _regionsController.UpdateRegion(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var badRequestResult = DTOResult as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResult.Value, badRequestResult.Value);
    }
}