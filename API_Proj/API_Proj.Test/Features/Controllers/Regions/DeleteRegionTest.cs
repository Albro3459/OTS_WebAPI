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
public class DeleteRegionTest {

    private RegionsController _regionsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public DeleteRegionTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _regionsController = new RegionsController(_mediatorMock.Object);

    }

    // DELETE Tests:
    [TestMethod]
    public async Task DeleteRegion_Valid_ShouldReturnNoContent() {
        //Arrange
        var expectedResultDTO = _fixture.Build<Region>()
                                .Without(r => r.Offices)
                                .Create();
        var expectedResultMessage = new NoContentResult();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteRegion.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _regionsController.DeleteRegion(expectedResultDTO.RegionID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(noContentResult.StatusCode, expectedResultMessage.StatusCode);
    }

    [TestMethod]
    public async Task DeleteRegion_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Region doesn't exist");
        var inValidID = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteRegion.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _regionsController.DeleteRegion(inValidID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(notFoundResult.Value, expectedResultMessage.Value);
    }
}