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
public class DeleteOfficeTest {

    private OfficesController _officesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public DeleteOfficeTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _officesController = new OfficesController(_mediatorMock.Object);
    }

    // DELETE Tests:
    [TestMethod]
    public async Task DeleteOffice_Valid_ShouldReturnNoContent() {
        //Arrange
        var expectedResultDTO = _fixture.Build<Office>()
                                .Without(o => o.Region)
                                .Without(o => o.Employees)
                                .Create();
        var expectedResultMessage = new NoContentResult();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteOffice.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _officesController.DeleteOffice(expectedResultDTO.OfficeID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(noContentResult.StatusCode, expectedResultMessage.StatusCode);
    }

    [TestMethod]
    public async Task DeleteOffice_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Office doesn't exist");
        var inValidID = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteOffice.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _officesController.DeleteOffice(inValidID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(notFoundResult.Value, expectedResultMessage.Value);
    }
}