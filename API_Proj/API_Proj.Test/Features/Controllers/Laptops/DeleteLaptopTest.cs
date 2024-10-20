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
public class DeleteLaptopTest {

    private LaptopsController _laptopsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public DeleteLaptopTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _laptopsController = new LaptopsController(_mediatorMock.Object);

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