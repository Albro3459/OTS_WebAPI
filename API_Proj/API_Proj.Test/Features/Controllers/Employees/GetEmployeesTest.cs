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
public class GetEmployeesTest {

    private EmployeesController _employeesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public GetEmployeesTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _employeesController = new EmployeesController(_mediatorMock.Object);
    }

    // GET Tests:
    [TestMethod]
    public async Task GetEmployee_ShouldReturnEmployeeDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<EmployeeDTO>>>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetEmployee.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _employeesController.GetEmployee(default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<EmployeeDTO>>>(DTOResult);
    }
}