﻿using Microsoft.AspNetCore.Mvc;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;
using API_Proj.Features.Request.Offices;
using API_Proj.Domain.Entity;

namespace API_Proj.Test.Features.Controllers.Offices;

[TestClass]
public class GetOfficeByIDTest {

    private OfficesController _officesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetOfficeByIDTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _officesController = new OfficesController(_mediatorMock.Object);
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
}