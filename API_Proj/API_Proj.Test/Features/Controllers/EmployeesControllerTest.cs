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
using Moq;
using AutoFixture;
using API_Proj.Features.Request.Laptops;
using API_Proj.Features.Request.Employees;

namespace API_Proj.Test.Features.Controllers;

[TestClass]
public class EmployeesControllerTest {

    private EmployeesController _employeesController;
    private Mock<IMediator> _mediatorMock;
    private Fixture _fixture;

    public EmployeesControllerTest() {
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
