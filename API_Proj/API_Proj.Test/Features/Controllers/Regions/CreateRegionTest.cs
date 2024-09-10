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
public class CreateRegionTest {

    private RegionsController _regionsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public CreateRegionTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _regionsController = new RegionsController(_mediatorMock.Object);

    }

    // CREATE Tests:
    [TestMethod]
    public async Task CreateRegion_Valid_ShouldReturnRegionDTO() {
        //Arrange
        var dtoToCreate = _fixture.Create<RegionForCreationDTO>();
        var expectedDTO = _fixture.Create<RegionDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateRegion.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _regionsController.CreateRegion(dtoToCreate, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<RegionDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task CreateRegion_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResultMessage = new BadRequestObjectResult("Region can't be null");

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateRegion.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _regionsController.CreateRegion(null, default);

        //Assert
        Assert.IsInstanceOfType<BadRequestObjectResult>(DTOResult.Result);

        var badRequestResult = DTOResult.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResultMessage.Value, badRequestResult.Value);
    }
}