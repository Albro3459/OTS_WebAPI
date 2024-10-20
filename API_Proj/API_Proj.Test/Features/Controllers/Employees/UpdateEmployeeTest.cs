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
public class UpdateEmployeeTest {

    private EmployeesController _employeesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public UpdateEmployeeTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _employeesController = new EmployeesController(_mediatorMock.Object);
    }

    // UPDATE Tests:
    [TestMethod]
    public async Task UpdateEmployee_Valid_ShouldReturnOk() {
        //Arrange
        var expectedResultDTO = _fixture.Create<EmployeeDTO>();
        var expectedResult = new OkObjectResult(expectedResultDTO);
        var dtoToUpdate = _fixture.Create<EmployeeDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateEmployee.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _employeesController.UpdateEmployee(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var okResult = DTOResult as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedResult.Value, okResult.Value);
    }

    [TestMethod]
    public async Task UpdateEmployee_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResult = new BadRequestObjectResult("Employee can't be null");
        var dtoToUpdate = _fixture.Create<EmployeeDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateEmployee.Query>(), default)).ReturnsAsync(expectedResult);
        var DTOResult = await _employeesController.UpdateEmployee(dtoToUpdate, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(DTOResult);
        var badRequestResult = DTOResult as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResult.Value, badRequestResult.Value);
    }
}