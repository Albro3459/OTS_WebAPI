using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using API_Proj.Infastructure;
using AutoMapper;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;
using API_Proj.Features.Request.Laptops;
using API_Proj.Features.Request.Offices;
using API_Proj.Features.Request.Employees;

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
    public async Task CreateOffice_InValid_ShouldReturnNotFound() {
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
}
