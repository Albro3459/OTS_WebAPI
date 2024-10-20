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
public class CreateOfficeTest {

    private OfficesController _officesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public CreateOfficeTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _officesController = new OfficesController(_mediatorMock.Object);
    }

    // CREATE Tests:
    [TestMethod]
    public async Task CreateOffice_Valid_ShouldReturnOfficeDTO() {
        //Arrange
        var dtoToCreate = _fixture.Create<OfficeForCreationDTO>();
        var expectedDTO = _fixture.Create<OfficeDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOffice.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _officesController.CreateOffice(dtoToCreate, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<OfficeDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task CreateOffice_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResultMessage = new BadRequestObjectResult("Office can't be null");

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOffice.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _officesController.CreateOffice(null, default);

        //Assert
        Assert.IsInstanceOfType<BadRequestObjectResult>(DTOResult.Result);

        var badRequestResult = DTOResult.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResultMessage.Value, badRequestResult.Value);
    }
}