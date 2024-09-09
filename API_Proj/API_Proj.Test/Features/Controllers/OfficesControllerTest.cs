using Microsoft.AspNetCore.Mvc;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;
using API_Proj.Features.Request.Offices;
using API_Proj.Domain.Entity;

namespace API_Proj.Test.Features.Controllers;

[TestClass]
public class OfficesControllerTest {

    private OfficesController _officesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public OfficesControllerTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _officesController = new OfficesController(_mediatorMock.Object);
    }

    // GET Tests:
    [TestMethod]
    public async Task GetOffice_ShouldReturnOfficeDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<OfficeDTO>>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOffice.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _officesController.GetOffice(default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<OfficeDTO>>>(DTOResult);
    }

    [TestMethod]
    public async Task GetOfficeByID_Valid_ShouldReturnOfficeDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<OfficeDTO>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOfficeByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _officesController.GetOfficeByID(expectedDTO.Value.OfficeID, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<OfficeDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetOfficeByID_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Office not found");
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOfficeByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _officesController.GetOfficeByID(id, default);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
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
