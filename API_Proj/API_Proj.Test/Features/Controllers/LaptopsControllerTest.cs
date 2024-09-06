using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using API_Proj.Infastructure;
using AutoMapper;
using API_Proj.Features;
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

    [TestMethod]
    public async Task GetLaptop_ShouldReturnLaptopDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<LaptopDTO>>>();
        var mockCancellationToken = CancellationToken.None;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetLaptop.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _laptopsController.GetLaptop(mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<LaptopDTO>>>(DTOResult);
    }

    [TestMethod]
    public async Task GetLaptopByID_Valid_ShouldReturnLaptopDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<LaptopDTO>>();
        var mockCancellationToken = CancellationToken.None;
        int id = 1001;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetLaptopByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _laptopsController.GetLaptopByID(id, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<LaptopDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetLaptopByID_InValid_ShouldReturnLaptopDTO() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Laptop not found");
        var mockCancellationToken = CancellationToken.None;
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetLaptopByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _laptopsController.GetLaptopByID(id, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }
}