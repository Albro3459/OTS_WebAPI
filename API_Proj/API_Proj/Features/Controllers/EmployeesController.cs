using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Proj.Domain.Entity;
using API_Proj.Infastructure;
using API_Proj.Features.DTO;
using API_Proj.Features;
using AutoMapper;
using System.Security.Cryptography;
using NuGet.Packaging;
using MediatR;
using API_Proj.Features.Request.Employees;
using API_Proj.Features.Request.Laptops;
using System.Threading;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Employees/Get/
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee(CancellationToken cancellationToken)
        {
            var employees = await _mediator.Send(new GetEmployee.Query(), cancellationToken);

            if (employees.Value != null)
            {
                return Ok(employees.Value);
            }

            return employees.Result ?? BadRequest();
        }

        // PUT: api/Employees/Get/{id}
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeByID(int id, CancellationToken cancellationToken) {
            var employee = await _mediator.Send(new GetEmployeeByID.Query(id), cancellationToken);

            if (employee.Value != null) {
                return Ok(employee.Value);
            }

            return employee.Result ?? BadRequest();
        }

        // PUT: api/Employees/Update/
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateEmployee(EmployeeDTO _employee, CancellationToken cancellationToken) 
        {
            var employee = await _mediator.Send(new UpdateEmployee.Query(_employee), cancellationToken);

            if (employee.Value != null)
            {
                return Ok(employee.Value);
            }

            return employee.Result ?? BadRequest();
        }

        //POST: api/Employees/Create
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody] EmployeeForCreationDTO _employee, CancellationToken cancellationToken)
        {
            var employee = await _mediator.Send(new CreateEmployee.Query(_employee), cancellationToken);

            if (employee.Value != null)
            {
                return Ok(employee.Value);
            }

            return employee.Result ?? BadRequest();
        }

        // DELETE: api/Employees/Delete/5
        [HttpDelete("Delete/{employeeID}")]
        public async Task<IActionResult> DeleteEmployee(int employeeID, CancellationToken cancellationToken) {
            var result = await _mediator.Send(new DeleteEmployee.Query(employeeID), cancellationToken);

            return result.Result ?? BadRequest();
        }
    }
}
