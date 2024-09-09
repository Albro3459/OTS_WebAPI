﻿using System;
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
using API_Proj.Features.Request.Regions;
using API_Proj.Features.Request.Offices;

namespace API_Proj.Test.Features.Controllers;

[TestClass]
public class RegionsControllerTest {

    private RegionsController _regionsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public RegionsControllerTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _regionsController = new RegionsController(_mediatorMock.Object);

    }

    // GET Tests:
    [TestMethod]
    public async Task GetRegion_ShouldReturnRegionDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<RegionDTO>>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegion.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _regionsController.GetRegion(default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<RegionDTO>>>(DTOResult);
    }

    [TestMethod]
    public async Task GetRegionByID_Valid_ShouldReturnRegionDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<RegionDTO>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegionByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _regionsController.GetRegionByID(expectedDTO.Value.RegionID, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<RegionDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetRegionByID_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Region not found");
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegionByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _regionsController.GetRegionByID(id, default);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }

    // CREATE Tests:
    [TestMethod]
    public async Task CreateRegion_Valid_ShouldReturnRegionDTO() {
        //Arrange
        var dtoToCreate = _fixture.Create<RegionForCreationDTO>();
        var expectedDTO = _fixture.Create<RegionDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateRegion.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _regionsController.CreateRegion(dtoToCreate, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<RegionDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task CreateRegion_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new BadRequestObjectResult("Region can't be null");

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateRegion.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _regionsController.CreateRegion(null, default);

        //Assert
        Assert.IsInstanceOfType<BadRequestObjectResult>(DTOResult.Result);

        var badRequestResult = DTOResult.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResultMessage.Value, badRequestResult.Value);
    }

}
