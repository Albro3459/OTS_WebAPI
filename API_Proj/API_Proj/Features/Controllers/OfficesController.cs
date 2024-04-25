using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Proj.Domain.Entity;
using API_Proj.Infastructure;
using AutoMapper;
using API_Proj.Features.DTO;
using System.Drawing;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public OfficesController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Offices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfficeDTO>>> GetOffice()
        {
            var offices = await _context.Office
                .Include(o => o.Employees)
                .Select(o => _mapper.Map<OfficeDTO>(o))
                .ToListAsync();

            return offices;
        }

        // GET: api/Offices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeDTO>> GetOffice(int id)
        {
            var office = await _context.Office
                .Include(o => o.Employees)
                .Where(o => o.OfficeID == id)
                .Select(o => _mapper.Map<OfficeDTO>(o))
                .SingleOrDefaultAsync();

            if (office == null)
            {
                return NotFound();
            }

            return office;
        }

        // PUT: api/Offices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffice(int id, Office office)
        {
            if (id != office.OfficeID)
            {
                return BadRequest();
            }

            _context.Entry(office).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeExists(id))
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

        // POST: api/Offices
        [HttpPost("Create")]
        public async Task<ActionResult<OfficeDTO>> CreateOffice(OfficeForCreationDTO _office)
        {

            if (_office == null)
            {
                return NotFound("Office can't be null");
            }

            var Office = _mapper.Map<Office>(_office);

            if (_office.EmployeesIDs != null && _office.EmployeesIDs.Count != 0)
            {
                foreach (var id in _office.EmployeesIDs)
                {
                    var employee = await _context.Employee.Where(e => e.EmployeeID == id).FirstOrDefaultAsync();
                    if (employee == null)
                    {
                        return NotFound("Employees doesn't exist");
                    }

                    if (employee.Offices.Contains(Office))
                    {
                        continue;
                        //return BadRequest("Employee already works at that office");
                    }
                    Office.Employees.Add(employee);

                }
            }

            if (_office.RegionID != null)
            {
                var region = await _context.Region.Where(r => r.RegionID == _office.RegionID).FirstOrDefaultAsync();
                if (region == null)
                {
                    return NotFound("Region doesn't exist");
                }
                if (!region.Offices.Contains(Office))
                {
                    Office.Region = region;
                }
                
                region.Offices.Add(Office);
            }

            // Uneccesary but works, ef core does it for me
            //foreach (var e in Office.Employees)
            //{
            //    e.Offices.Add(Office);
            //}

            _context.Office.Add(Office);
            await _context.SaveChangesAsync();

            var officeDTO = await _context.Office
                .Where(o => o.OfficeID == Office.OfficeID)
                .Select(o => _mapper.Map<OfficeDTO>(o))
                .FirstOrDefaultAsync();

            return officeDTO ?? new OfficeDTO();
        }



        // DELETE: api/Offices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffice(int id)
        {
            var office = await _context.Office.FindAsync(id);
            if (office == null)
            {
                return NotFound();
            }

            _context.Office.Remove(office);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfficeExists(int id)
        {
            return _context.Office.Any(e => e.OfficeID == id);
        }
    }
}
