using Microsoft.AspNetCore.Mvc;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;
using API_Proj.Features.Request.Offices;
using API_Proj.Domain.Entity;

namespace API_Proj.Test.Features.Controllers.Offices;

[TestClass]
public class UnassignOfficeRegionTest {

    private OfficesController _officesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public UnassignOfficeRegionTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _officesController = new OfficesController(_mediatorMock.Object);
    }

    [TestMethod]
    public async Task UnassignOfficeRegion_Valid_ShouldReturnOk() {
        //Arrange
        var expectedResultDTO = _fixture.Create<OfficeDTO>();
        var expectedResult = new OkObjectResult(expectedResultDTO);
        var dtoToUpdate = _fixture.Create<OfficeDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UnassignOfficeRegion.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _officesController.UnassignOfficeRegion(dtoToUpdate.OfficeID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var okResult = DTOResult as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult.Value, okResult.Value);
    }

    [TestMethod]
    public async Task UnassignOfficeRegion_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResult = new NotFoundObjectResult("Office doesn't exist");
        var idToUpdate = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UnassignOfficeRegion.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _officesController.UnassignOfficeRegion(idToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var notFoundResult = DTOResult as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResult.Value, notFoundResult.Value);
    }
}