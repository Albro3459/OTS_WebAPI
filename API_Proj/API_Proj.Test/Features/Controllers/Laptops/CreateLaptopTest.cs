using Microsoft.AspNetCore.Mvc;
using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Features.Request.Laptops;
using MediatR;
using API_Proj.Features.Controllers;
using Moq;
using AutoFixture;

namespace API_Proj.Test.Features.Controllers.Laptops;

[TestClass]
public class CreateLaptopTest {

    private LaptopsController _laptopsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public CreateLaptopTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _laptopsController = new LaptopsController(_mediatorMock.Object);

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
}