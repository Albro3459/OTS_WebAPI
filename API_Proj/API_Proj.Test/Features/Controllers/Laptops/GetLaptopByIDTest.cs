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
public class GetLaptopByIDTest {

    private LaptopsController _laptopsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetLaptopByIDTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _laptopsController = new LaptopsController(_mediatorMock.Object);

    }
    //GET Tests:
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
}