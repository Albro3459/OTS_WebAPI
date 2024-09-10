using Microsoft.AspNetCore.Mvc;
using API_Proj.Features.DTO;
using MediatR;
using API_Proj.Features.Controllers;
using Moq;
using AutoFixture;
using API_Proj.Features.Request.Employees;
using API_Proj.Domain.Entity;

namespace API_Proj.Test.Features.Controllers.Employees;

[TestClass]
public class GetEmployeeByIDTest {

    private EmployeesController _employeesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetEmployeeByIDTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _employeesController = new EmployeesController(_mediatorMock.Object);
    }

    // GET Tests:
    [TestMethod]
    public async Task GetEmployeeByID_Valid_ShouldReturnEmployeeDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<EmployeeDTO>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetEmployeeByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _employeesController.GetEmployeeByID(expectedDTO.Value.EmployeeID, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<EmployeeDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetEmployeeByID_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Employee not found");
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetEmployeeByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _employeesController.GetEmployeeByID(id, default);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }
}
