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
public class CreateEmployeeTest {

    private EmployeesController _employeesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public CreateEmployeeTest() {
        _fixture = new Fixture();
        _mediatorMock = new Mock<IMediator>();
        _employeesController = new EmployeesController(_mediatorMock.Object);
    }

    // CREATE Tests:
    [TestMethod]
    public async Task CreateEmployee_Valid_ShouldReturnEmployeeDTO() {
        //Arrange
        var dtoToCreate = _fixture.Create<EmployeeForCreationDTO>();
        var expectedDTO = _fixture.Create<EmployeeDTO>();

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateEmployee.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _employeesController.CreateEmployee(dtoToCreate, default);

        //Assert
        Assert.IsInstanceOfType<ActionResult<EmployeeDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task CreateEmployee_InValid_ShouldReturnBadRequest() {
        //Arrange
        var expectedResultMessage = new BadRequestObjectResult("Employee can't be null");

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateEmployee.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _employeesController.CreateEmployee(null, default);

        //Assert
        Assert.IsInstanceOfType<BadRequestObjectResult>(DTOResult.Result);

        var badRequestResult = DTOResult.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(expectedResultMessage.Value, badRequestResult.Value);
    }
}