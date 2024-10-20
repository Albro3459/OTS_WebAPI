using Microsoft.AspNetCore.Mvc;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using AutoFixture;
using Moq;
using API_Proj.Features.Request.Offices;
using API_Proj.Domain.Entity;

namespace API_Proj.Test.Features.Controllers.Offices;

[TestClass]
public class GetOfficeTest {

    private OfficesController _officesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetOfficeTest() {
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
}