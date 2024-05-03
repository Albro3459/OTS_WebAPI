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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.JsonPatch;
using System.Drawing;
using API_Proj.Features.Request.Laptops;
using MediatR;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LaptopsController(ApiDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: api/Laptops/Get
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<LaptopDTO>>> GetLaptop()
        {
            var laptops = await _mediator.Send(new GetLaptop.Query());

            return laptops;
        }


        // GET: api/Laptops/Get/1001
        //[HttpGet("Get/{id}")]
        //public async Task<ActionResult<LaptopDTO>> GetLaptopByID(int id)
        //{
        //    var laptop = await _context.Laptop
        //        .Where(l => l.LaptopID == id)
        //        .Select(l => _mapper.Map<LaptopDTO>(l))
        //        .SingleOrDefaultAsync();

        //    if (laptop == null)
        //    {
        //        return NotFound();
        //    }

        //    return laptop;
        //}

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<LaptopDTO>> GetLaptopByID(int id)
        {
            var laptop = await _mediator.Send(new GetLaptopByID.Query(id));

            if (laptop == null)
            {
                return NotFound();
            }

            return laptop;
        }


        //PUT: api/Laptops/Update/
        [HttpPut("Update/")]
        public async Task<IActionResult> UpdateLaptop(LaptopDTO _laptop)
        {
            if (_laptop == null)
            {
                return NotFound("Laptop can't be null");
            }

            var oldLaptop = await _context.Laptop
                .Include(l => l.Employee)
                .Where(l => l.LaptopID == _laptop.LaptopID).FirstOrDefaultAsync();

            if (oldLaptop == null)
            {
                return NotFound("Laptop doesn't exist");
            }

            _mapper.Map(_laptop, oldLaptop);

            if (_laptop.EmployeeID != null)
            {
                if (oldLaptop.Employee == null || oldLaptop.Employee.EmployeeID != _laptop.EmployeeID)
                {    var employee = await _context.Employee
                        .Include(e => e.Offices)
                        .ThenInclude(o => o.Region)
                        .Include(e => e.Offices)
                        .ThenInclude(o => o.Employees)
                        .ThenInclude(e => e.Laptop)
                        .Where(e => e.EmployeeID == _laptop.EmployeeID).FirstOrDefaultAsync();

                    if (employee == null) { return BadRequest("Employee doesn't exist"); }

                    oldLaptop.EmployeeID = _laptop.EmployeeID;
                    oldLaptop.Employee = employee;
                }

            }

            _context.Update(oldLaptop);
            await _context.SaveChangesAsync();

            var returnLaptop = _mapper.Map<LaptopDTO>(oldLaptop);
            return Ok(returnLaptop);

        }

        //PUT: api/Laptops/Update/{laptopID}/UnassignOwner
        [HttpPut("Update/{laptopID}/UnassignOwner")]
        public async Task<IActionResult> UnassignOwner(int laptopID)
        {
            var laptop = await _context.Laptop
                .Include(l => l.Employee)
                .Where(l => l.LaptopID == laptopID).FirstOrDefaultAsync();

            if (laptop == null)
            {
                return NotFound("Laptop doesn't exist");
            }

            if (laptop.EmployeeID == null || laptop.Employee == null)
            {
                return NotFound("Laptop doesn't have an employee");
            }

            var employee = await _context.Employee.Where(e => e.EmployeeID == laptop.EmployeeID).FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound("Employee doesn't exist");
            }

            laptop.Employee = null;
            laptop.EmployeeID = null;

            
            employee.Laptop = null;

            await _context.SaveChangesAsync();

            var laptopDTO = _mapper.Map<LaptopDTO>(laptop);
            return Ok(laptopDTO);

        }

        // POST: api/Laptops/
        [HttpPost("Create")]
        public async Task<ActionResult<LaptopDTO>> CreateLaptop([FromBody] LaptopForCreationDTO _laptop)
        {

            if (_laptop == null)
            {
                return NotFound("Laptop can't be null");
            }

            var Laptop = _mapper.Map<Laptop>(_laptop);

            if (_laptop.EmployeeID != null)
            {
              
                var employee = await _context.Employee
                    .Include(e => e.Offices)
                    .Where(e => e.EmployeeID == _laptop.EmployeeID).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                if (employee.Laptop != null)
                {
                    return BadRequest("Employee already has a laptop");
                }

                employee.Laptop = Laptop;
            }

            _context.Laptop.Add(Laptop);

            await _context.SaveChangesAsync();

            //var laptopDTO = await _context.Laptop
            //    .Where(l => l.LaptopID == Laptop.LaptopID)
            //    .Select(l => _mapper.Map<LaptopDTO>(l))
            //    .FirstOrDefaultAsync();

            var laptopDTO = _mapper.Map<LaptopDTO>(Laptop);

            return laptopDTO ?? new LaptopDTO();
        }

        // DELETE: api/Laptops/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteLaptop(int id)
        {
            var laptop = await _context.Laptop.FindAsync(id);
            if (laptop == null)
            {
                return NotFound("Laptop doesn't exist");
            }


            laptop.IsDeleted = true;
            _context.Update(laptop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptop.Any(e => e.LaptopID == id);
        }
    }
}
