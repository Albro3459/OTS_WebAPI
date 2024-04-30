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

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {
            var employees = await _context.Employee
                .Include(e => e.Offices)
                .Include(e => e.Laptop)
                .Select(e => _mapper.Map<EmployeeDTO>(e))
                .ToListAsync();

            return employees;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employee
                .Include(e => e.Offices)
                .Include(e => e.Laptop)
                .Where(e => e.EmployeeID == id)
                .Select(e => _mapper.Map<EmployeeDTO>(e))
                .SingleOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/Update/
        [HttpPut("Update/")]
        public async Task<IActionResult> UpdateEmployee(EmployeeDTO _employee)
        {
            if (_employee == null)
            { return NotFound("Employee can't be null"); }

            var oldEmployee = await _context.Employee
                .Include(e => e.Laptop)
                .Include(e => e.Offices)
                .ThenInclude(o => o.Region)
                .ThenInclude(r => r.Offices)
                .Where(e => e.EmployeeID == _employee.EmployeeID).FirstOrDefaultAsync();

            if (oldEmployee == null)
            { return NotFound("Employee doesn't exist"); }


            _mapper.Map(_employee, oldEmployee);


            if (_employee.OfficesIDs != null && _employee.OfficesIDs.Count >= 0)
            {
                oldEmployee.Offices.Clear();

                foreach (var id in _employee.OfficesIDs)
                {
                    var office = await _context.Office.Where(o => o.OfficeID == id).FirstOrDefaultAsync();
                    if (office == null) { continue; }
                    oldEmployee.Offices.Add(office);
                }
                
            }

            if (_employee.LaptopID != null)
            {
                if (oldEmployee.Laptop == null || oldEmployee.Laptop.LaptopID != _employee.LaptopID) {

                    var laptop = await _context.Laptop.Where(l => l.LaptopID == _employee.LaptopID).FirstOrDefaultAsync();

                    if (laptop == null) { return NotFound("Laptop doesn't exist"); }

                    oldEmployee.Laptop = laptop;
                }

            }

            _context.Update(oldEmployee);
            await _context.SaveChangesAsync();

            var employeeDTO = _mapper.Map<EmployeeDTO>(oldEmployee);

            return Ok(employeeDTO);
            
        }

        //POST: api/Employees
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody] EmployeeForCreationDTO _employee)
        {

            if (_employee == null) {
                return NotFound("Employee can't be null");
            }

            var Employee = _mapper.Map<Employee>(_employee);

            if (_employee.LaptopID != null)
            {

                var laptop = await _context.Laptop
                    .Include(l => l.Employee)
                    .ThenInclude(e => e.Offices)
                    .ThenInclude(o => o.Region)
                    .Where(l => l.LaptopID == _employee.LaptopID).FirstOrDefaultAsync();
                
                if (laptop == null)
                {
                    return NotFound("Employee not found");
                }

                if (laptop.EmployeeID != null)
                {
                    return NotFound("Laptop is taken");
                }

                Employee.Laptop = laptop;
            }

            if (_employee.OfficesIDs != null && _employee.OfficesIDs.Count != 0)
            {
                var offices = new List<Office>();

                foreach (var id in _employee.OfficesIDs) {

                    var office = await _context.Office
                        .Include(o => o.Employees)
                        .ThenInclude(e => e.Laptop)
                        .Include(o => o.Employees)
                        .ThenInclude(e => e.Offices)
                        .ThenInclude(o => o.Region)
                        .Include(o => o.Region)
                        .ThenInclude(r => r.Offices)
                        .Where(o => o.OfficeID == id).FirstOrDefaultAsync();
                    
                    if (office == null)
                    {
                        return NotFound("Office not found");
                    }
                    office.Employees.Add(Employee);
                }
            }

            _context.Employee.Add(Employee);

            await _context.SaveChangesAsync();

            //var employeeDTO = await _context.Employee
            //    .Where(e => e.EmployeeID == Employee.EmployeeID)
            //    .Select(e => _mapper.Map<EmployeeDTO>(e))
            //    .FirstOrDefaultAsync();

            var employeeDTO = _mapper.Map<EmployeeDTO>(Employee);


            return employeeDTO ?? new EmployeeDTO();
        }

        // DELETE: api/Employees/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null || employee.IsDeleted)
            {
                return NotFound("Employee doesn't exist");
            }


            employee.IsDeleted = true;
            var laptop = await _context.Laptop.Where(l => l.EmployeeID == id).FirstOrDefaultAsync();
            if (laptop != null)
            {
                laptop.Employee = null;
                laptop.EmployeeID = null;
            }

            //_context.Update(employee);


            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeID == id);
        }
    }
}
