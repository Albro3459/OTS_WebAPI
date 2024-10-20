using Microsoft.AspNetCore.Mvc;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;
using API_Proj.Features.Request.Regions;
using API_Proj.Domain.Entity;

namespace API_Proj.Test.Features.Controllers.Regions;

[TestClass]
public class GetRegionTest {

    private RegionsController _regionsController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetRegionTest() {
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
}