using Microsoft.AspNetCore.Mvc;
using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Features.Request.Laptops;
using MediatR;
using API_Proj.Features.Controllers;
using Moq;
using AutoFixture;

namespace API_Proj.Test.Features.Controllers;

[TestClass]
public class LaptopsControllerTest {

    private LaptopsController _laptopsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public LaptopsControllerTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _laptopsController = new LaptopsController(_mediatorMock.Object);

    }
    //GET Tests:
    [TestMethod]
    public async Task GetLaptop_ShouldReturnLaptopDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<LaptopDTO>>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetLaptop.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _laptopsController.GetLaptop(default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<LaptopDTO>>>(DTOResult);
    }

    [TestMethod]
    public async Task GetLaptopByID_Valid_ShouldReturnLaptopDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<LaptopDTO>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetLaptopByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _laptopsController.GetLaptopByID(expectedDTO.Value.LaptopID, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<LaptopDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetLaptopByID_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Laptop not found");
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetLaptopByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _laptopsController.GetLaptopByID(id, default);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }

    // UPDATE Tests:
    [TestMethod]
    public async Task UpdateLaptop_Valid_ShouldReturnOk() {
        //Arrange
        var expectedResultDTO = _fixture.Create<LaptopDTO>();
        var expectedResult = new OkObjectResult(expectedResultDTO);
        var dtoToUpdate = _fixture.Create<LaptopDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateLaptop.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _laptopsController.UpdateLaptop(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var okResult = DTOResult as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult.Value, okResult.Value);
    }

    [TestMethod]
    public async Task UpdateLaptop_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResult = new BadRequestObjectResult("Laptop can't be null");
        var dtoToUpdate = _fixture.Create<LaptopDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateLaptop.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _laptopsController.UpdateLaptop(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var badRequestResult = DTOResult as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResult.Value, badRequestResult.Value);
    }

    [TestMethod]
    public async Task UnassignLaptopOwner_Valid_ShouldReturnOk() {
        //Arrange
        var expectedResultDTO = _fixture.Create<LaptopDTO>();
        var expectedResult = new OkObjectResult(expectedResultDTO);
        var dtoToUpdate = _fixture.Create<LaptopDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UnassignLaptopOwner.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _laptopsController.UnassignLaptopOwner(dtoToUpdate.LaptopID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var okResult = DTOResult as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult.Value, okResult.Value);
    }

    [TestMethod]
    public async Task UnassignLaptopOwner_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResult = new NotFoundObjectResult("Laptop doesn't exist");
        var idToUpdate = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UnassignLaptopOwner.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _laptopsController.UnassignLaptopOwner(idToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var notFoundResult = DTOResult as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResult.Value, notFoundResult.Value);
    }



    // CREATE Tests:
    [TestMethod]
    public async Task CreateLaptop_Valid_ShouldReturnLaptopDTO() {
        //Arrange
        var dtoToCreate = _fixture.Create<LaptopForCreationDTO>();
        var expectedDTO = _fixture.Create<LaptopDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLaptop.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _laptopsController.CreateLaptop(dtoToCreate, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<LaptopDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task CreateLaptop_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResultMessage = new BadRequestObjectResult("Laptop can't be null");

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLaptop.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _laptopsController.CreateLaptop(null, default);

        //Assert
        Assert.IsInstanceOfType<BadRequestObjectResult>(DTOResult.Result);

        var badRequestResult = DTOResult.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResultMessage.Value, badRequestResult.Value);
    }

    // DELETE Tests:
    [TestMethod]
    public async Task DeleteLaptop_Valid_ShouldReturnNoContent() {
        //Arrange
        var expectedResultDTO = _fixture.Build<Laptop>()
                                .Without(l => l.Employee)
                                .Create();
        var expectedResultMessage = new NoContentResult();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteLaptop.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _laptopsController.DeleteLaptop(expectedResultDTO.LaptopID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(noContentResult.StatusCode, expectedResultMessage.StatusCode);
    }

    [TestMethod]
    public async Task DeleteLaptop_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Laptop doesn't exist");
        var inValidID = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteLaptop.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _laptopsController.DeleteLaptop(inValidID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(notFoundResult.Value, expectedResultMessage.Value);
    }
}