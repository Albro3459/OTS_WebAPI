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

    [TestMethod]
    public async Task GetRegion_ShouldReturnRegionDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<RegionDTO>>>();
        var mockCancellationToken = CancellationToken.None;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegion.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _regionsController.GetRegion(mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<RegionDTO>>>(DTOResult);
    }

    [TestMethod]
    public async Task GetRegionByID_Valid_ShouldReturnRegionDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<RegionDTO>>();
        var mockCancellationToken = CancellationToken.None;
        int id = 1001;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegionByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _regionsController.GetRegionByID(id, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<RegionDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetRegionByID_InValid_ShouldReturnRegionDTO() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Region not found");
        var mockCancellationToken = CancellationToken.None;
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetRegionByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _regionsController.GetRegionByID(id, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }

}
