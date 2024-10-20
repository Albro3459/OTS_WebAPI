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
public class GetRegionByIDTest {

    private RegionsController _regionsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetRegionByIDTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _regionsController = new RegionsController(_mediatorMock.Object);

    }

    [TestMethod]
    public async Task GetRegionByID_Valid_ShouldReturnRegionDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<RegionDTO>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegionByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _regionsController.GetRegionByID(expectedDTO.Value.RegionID, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<RegionDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetRegionByID_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Region not found");
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegionByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _regionsController.GetRegionByID(id, default);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }
}