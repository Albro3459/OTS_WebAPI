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
public class UpdateLaptopTest {

    private LaptopsController _laptopsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public UpdateLaptopTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _laptopsController = new LaptopsController(_mediatorMock.Object);

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
}