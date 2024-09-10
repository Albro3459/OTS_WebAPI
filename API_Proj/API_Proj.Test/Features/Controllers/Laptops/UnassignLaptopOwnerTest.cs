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
public class UnassignLaptopOwnerTest {

    private LaptopsController _laptopsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public UnassignLaptopOwnerTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _laptopsController = new LaptopsController(_mediatorMock.Object);

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
}