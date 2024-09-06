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

    [TestMethod]
    public async Task GetEmployee_ShouldReturnEmployeeDTOs() {
        //Arrange
        var expectedDTOs = _fixture.Create<ActionResult<IEnumerable<EmployeeDTO>>>();
        var mockCancellationToken = CancellationToken.None;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetEmployee.Query>(), default)).ReturnsAsync(expectedDTOs);
        var DTOResult = await _employeesController.GetEmployee(mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<IEnumerable<EmployeeDTO>>>(DTOResult);
    }

    [TestMethod]
    public async Task GetEmployeeByID_Valid_ShouldReturnEmployeeDTO() {
        //Arrange
        var expectedDTO = _fixture.Create<ActionResult<EmployeeDTO>>();
        var mockCancellationToken = CancellationToken.None;
        int id = 1001;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetEmployeeByID.Query>(), default)).ReturnsAsync(expectedDTO);
        var DTOResult = await _employeesController.GetEmployeeByID(id, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<ActionResult<EmployeeDTO>>(DTOResult);
    }

    [TestMethod]
    public async Task GetEmployeeByID_InValid_ShouldReturnEmployeeDTO() {
        //Arrange
        var expectedResultMessage = new NotFoundObjectResult("Employee not found");
        var mockCancellationToken = CancellationToken.None;
        int id = 0;

        //Act
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetEmployeeByID.Query>(), default)).ReturnsAsync(expectedResultMessage);
        var DTOResult = await _employeesController.GetEmployeeByID(id, mockCancellationToken);

        //Assert
        Assert.IsInstanceOfType<NotFoundObjectResult>(DTOResult.Result);

        var notFoundResult = DTOResult.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(expectedResultMessage.Value, notFoundResult.Value);
    }

}
