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
using MediatR;
using API_Proj.Features.Request.Office;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public OfficesController(ApiDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        // GET: api/Offices/Get
        [HttpGet("Get")]
        //public async Task<ActionResult<IEnumerable<OfficeDTO>>> GetOffice()
        //{
        //var offices = await _context.Office
        //        .Include(o => o.Employees).ThenInclude(e => e.Laptop)
        //        .Include(o => o.Region)
        //        .Select(o => _mapper.Map<OfficeDTO>(o))
        //        .ToListAsync();

        //    return offices;
        //}

        public async Task<ActionResult<IEnumerable<OfficeDTO>>> GetOffice()
        {
            var offices = await _mediator.Send(new GetOffice.Query());

            return offices;
        }


        // GET: api/Offices/Get/1001
        //[HttpGet("Get/{id}")]
        //public async Task<ActionResult<OfficeDTO>> GetOffice(int id)
        //{
        //    var office = await _context.Office
        //        .Include(o => o.Employees)
        //        .Include(o => o.Region)
        //        .Where(o => o.OfficeID == id)
        //        .Select(o => _mapper.Map<OfficeDTO>(o))
        //        .SingleOrDefaultAsync();

        //    if (office == null)
        //    {
        //        return NotFound();
        //    }

        //    return office;
        //}

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<OfficeDTO>> GetOfficeByID(int id)
        {
            var office = await _mediator.Send(new GetOfficeByID.Query(id));

            return office;
        }


        // PUT: api/Offices/Update/
        [HttpPut("Update/")]
        public async Task<IActionResult> UpdateOffice(OfficeDTO _office)
        {
            if (_office == null)
            { return NotFound("Office can't be null"); }

            var oldOffice = await _context.Office
                .Include(o => o.Employees)
                .ThenInclude(e => e.Laptop)
                .Include(o => o.Employees)
                .ThenInclude(e => e.Offices)
                .ThenInclude(o => o.Region)
                .Include(o => o.Region)
                .ThenInclude(r => r.Offices)
                .Where(o => o.OfficeID ==  _office.OfficeID).FirstOrDefaultAsync();

            if (oldOffice == null)
            { return NotFound("Office doesn't exist"); }


            _mapper.Map(_office, oldOffice);


            if (_office.EmployeesIDs != null && _office.EmployeesIDs.Count >= 0)
            { 
                oldOffice.Employees.Clear();

                foreach(var id in _office.EmployeesIDs)
                {
                    var employee = await _context.Employee.Where(e => e.EmployeeID == id).FirstOrDefaultAsync();
                    if (employee == null) { continue; }
                    oldOffice.Employees.Add(employee);
                }

            }

            if (_office.RegionID != null)
            {
                if (oldOffice.Region == null || oldOffice.Region.RegionID != _office.RegionID)
                {
                    var region = await _context.Region.Where(r => r.RegionID == _office.RegionID).FirstOrDefaultAsync();

                    if (region == null) { return NotFound("Region doesn't exist"); }

                    oldOffice.Region = region;
                }

            }

            _context.Update(oldOffice);
            await _context.SaveChangesAsync();

            var officeDTO = _mapper.Map<OfficeDTO>(oldOffice);

            return Ok(officeDTO);
        }

        //PUT: api/Offices/Update/{officeID}/UnassignRegion
        [HttpPut("Update/{officeID}/UnassignRegion")]
        public async Task<IActionResult> UnassignRegion(int officeID)
        {
            var office = await _context.Office
                .Include(o => o.Employees)
                .ThenInclude(e => e.Laptop)
                .Include(o => o.Employees)
                .ThenInclude(e => e.Offices)
                .ThenInclude(o => o.Region)
                .Include(o => o.Region)
                .ThenInclude(r => r.Offices)
                .Where(o => o.OfficeID == officeID).FirstOrDefaultAsync();

            if (office == null)
            {
                return NotFound("Office doesn't exist");
            }

            if (office.RegionID == null || office.Region == null)
            {
                return NotFound("Office doesn't have an region");
            }

            var region = await _context.Region.Where(r => r.RegionID == office.RegionID).FirstOrDefaultAsync();

            if (region == null)
            {
                return NotFound("Region doesn't exist");
            }

            office.Region = null;
            office.RegionID = null;

            if (region.Offices != null && region.Offices.Count > 0)
            {
                region.Offices.Remove(office);
            }


            await _context.SaveChangesAsync();

            var officeDTO = _mapper.Map<OfficeDTO>(office);
            return Ok(officeDTO);

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
                    var employee = await _context.Employee
                        .Include(e => e.Offices)
                        .ThenInclude(o => o.Region)
                        .Include(e => e.Offices)
                        .ThenInclude(o => o.Employees)
                        .Include(e => e.Laptop)
                        .Where(e => e.EmployeeID == id).FirstOrDefaultAsync();

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

            var officeDTO = _mapper.Map<OfficeDTO>(Office);

            return officeDTO ?? new OfficeDTO();
        }



        // DELETE: api/Offices/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOffice(int id)
        {
            var office = await _context.Office.FindAsync(id);
            if (office == null)
            {
                return NotFound("Office doesn't exist");
            }

            office.IsDeleted = true;
            _context.Update(office);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfficeExists(int id)
        {
            return _context.Office.Any(e => e.OfficeID == id);
        }
    }
}
