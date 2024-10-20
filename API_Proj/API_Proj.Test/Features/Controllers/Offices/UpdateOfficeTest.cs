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
public class UpdateOfficeTest {

    private OfficesController _officesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public UpdateOfficeTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _officesController = new OfficesController(_mediatorMock.Object);
    }

    // UPDATE Tests:
    [TestMethod]
    public async Task UpdateOffice_Valid_ShouldReturnOk() {
        //Arrange
        var expectedResultDTO = _fixture.Create<OfficeDTO>();
        var expectedResult = new OkObjectResult(expectedResultDTO);
        var dtoToUpdate = _fixture.Create<OfficeDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateOffice.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _officesController.UpdateOffice(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var okResult = DTOResult as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult.Value, okResult.Value);
    }

    [TestMethod]
    public async Task UpdateOffice_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResult = new BadRequestObjectResult("Office can't be null");
        var dtoToUpdate = _fixture.Create<OfficeDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateOffice.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _officesController.UpdateOffice(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var badRequestResult = DTOResult as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResult.Value, badRequestResult.Value);
    }
}