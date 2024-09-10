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
public class DeleteEmployeeTest {

    private EmployeesController _employeesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public DeleteEmployeeTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _employeesController = new EmployeesController(_mediatorMock.Object);
    }

    // DELETE Tests:
    [TestMethod]
    public async Task DeleteEmployee_Valid_ShouldReturnNoContent() {
        //Arrange
        var expectedResultDTO = _fixture.Build<Employee>()
                                .Without(e => e.Laptop)
                                .Without(e => e.Offices)
                                .Create();
        var expectedResultMessage = new NoContentResult();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteEmployee.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _employeesController.DeleteEmployee(expectedResultDTO.EmployeeID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(noContentResult.StatusCode, expectedResultMessage.StatusCode);
    }

    [TestMethod]
    public async Task DeleteEmployee_InValid_ShouldReturnNotFound() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Employee doesn't exist");
        var inValidID = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteEmployee.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var result = await _employeesController.DeleteEmployee(inValidID, default);

        //Assert
        Assert.IsInstanceOfType<IActionResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(notFoundResult.Value, expectedResultMessage.Value);
    }

}
