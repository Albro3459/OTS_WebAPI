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
using Microsoft.AspNetCore.Routing;

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
    public async Task CreateLaptop_InValid_ShouldReturnNotFound() {
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