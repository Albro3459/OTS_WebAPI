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

        // POST: api/Employees
        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeForCreationDTO>> CreateEmployee([FromBody] EmployeeForCreationDTO Employee)
        {
            if (Employee.OfficesIDs.Count == 0 || Employee.LaptopID == null)
            {
                return BadRequest("No offices or no laptop or both");
            }
            var offices = new List<Office>();

            foreach (var id in Employee.OfficesIDs) {
                var o = await _context.Office.Where(e => e.OfficeID == id).FirstOrDefaultAsync();
                if (o == null)
                {
                    return NotFound("Office not found");
                }
                else { offices.Add(o); }
            }

            var laptop = await _context.Laptop.Where(l => l.LaptopID == Employee.LaptopID).FirstOrDefaultAsync();
            if (laptop == null)
            {
                return NotFound("Laptop not found");
            }

            var employeeID = 1 + await _context.Employee.OrderBy(e => e.EmployeeID).Select(e => e.EmployeeID).FirstOrDefaultAsync();

            var employee = new Employee()
            {
                EmployeeID = employeeID,
                EmployeeName = Employee.EmployeeName,
                JobTitle = Employee.JobTitle,
                YearsAtCompany = Employee.YearsAtCompany,
                CurrentProjects = Employee.CurrentProjects,
                Offices = offices,
                Laptop = laptop
            };

            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, employee);
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
