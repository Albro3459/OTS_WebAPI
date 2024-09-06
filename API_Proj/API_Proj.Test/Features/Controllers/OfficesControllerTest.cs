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

    [TestMethod]
    public async Task GetOffice_ShouldReturnOfficeDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<OfficeDTO>>>();
        var mockCancellationToken = CancellationToken.None;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOffice.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _officesController.GetOffice(mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<OfficeDTO>>>(DTOResult);
    }

    [TestMethod]
    public async Task GetOfficeByID_Valid_ShouldReturnOfficeDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<OfficeDTO>>();
        var mockCancellationToken = CancellationToken.None;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOfficeByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _officesController.GetOfficeByID(expectedDTO.Value.OfficeID, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<OfficeDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetOfficeByID_InValid_ShouldReturnOfficeDTO() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Office not found");
        var mockCancellationToken = CancellationToken.None;
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOfficeByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _officesController.GetOfficeByID(id, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }
}
