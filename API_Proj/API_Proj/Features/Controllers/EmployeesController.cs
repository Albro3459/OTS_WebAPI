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

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/Employees
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody] EmployeeForCreationDTO _Employee)
        {

            if (_Employee == null) {
                return BadRequest("Employee can't be null");
            }

            var Employee = _mapper.Map<Employee>(_Employee);

            if (_Employee.LaptopID != null)
            {

                var laptop = await _context.Laptop.Where(l => l.LaptopID == _Employee.LaptopID).FirstOrDefaultAsync();
                if (laptop == null)
                {
                    return NotFound("Employee not found");
                }

                if (laptop.EmployeeID != null)
                {
                    return BadRequest("Laptop is taken");
                }

                Employee.Laptop = laptop;
            }

            if (_Employee.OfficesIDs != null && _Employee.OfficesIDs.Count != 0)
            {
                var offices = new List<Office>();

                foreach (var id in _Employee.OfficesIDs) {

                    var office = await _context.Office.Where(o => o.OfficeID == id).FirstOrDefaultAsync();
                    if (office == null)
                    {
                        return NotFound("Employee not found");
                    }
                    office.Employees.Add(Employee);
                }
            }

            _context.Employee.Add(Employee);

            await _context.SaveChangesAsync();

            var employeeDTO = await _context.Employee
                .Where(e => e.EmployeeID == Employee.EmployeeID)
                .Select(e => _mapper.Map<EmployeeDTO>(e))
                .FirstOrDefaultAsync();

            return employeeDTO ?? new EmployeeDTO();
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeID == id);
        }
    }
}
