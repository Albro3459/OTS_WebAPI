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
public class GetLaptopTest {

    private LaptopsController _laptopsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetLaptopTest() {
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
}