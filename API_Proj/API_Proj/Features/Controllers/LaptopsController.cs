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

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public LaptopsController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Laptops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaptopDTO>>> GetLaptop()
        {
            var laptops = await _context.Laptop
                .Select(l => _mapper.Map<LaptopDTO>(l))
                .ToListAsync();  

            return laptops;
        }

        // GET: api/Laptops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LaptopDTO>> GetLaptop(int id)
        {
            var laptop = await _context.Laptop
                .Where(l => l.LaptopID == id)
                .Select(l => _mapper.Map<LaptopDTO>(l))
                .SingleOrDefaultAsync();

            if (laptop == null)
            {
                return NotFound();
            }

            return laptop;
        }

        // PUT: api/Laptops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaptop(int id, Laptop laptop)
        {
            if (id != laptop.LaptopID)
            {
                return BadRequest();
            }

            _context.Entry(laptop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaptopExists(id))
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

        // POST: api/Laptops/
        [HttpPost("Create")]
        public async Task<ActionResult<LaptopForCreationDTO>> CreateLaptop([FromBody] LaptopForCreationDTO Laptop)
        {

            if (Laptop == null)
            {
                return BadRequest("Laptop invalid");
            }

            var laptopID = 1 + await _context.Laptop
                    .OrderBy(l => l.LaptopID)
                    .Select(l => l.LaptopID).FirstOrDefaultAsync();

            var newLaptop = new Laptop();

            if (Laptop.EmployeeID != null)
            {
                var employee = await _context.Employee.Where(e => e.EmployeeID == Laptop.EmployeeID).FirstOrDefaultAsync();
                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                newLaptop = new Laptop() { LaptopID = laptopID, LaptopName = Laptop.LaptopName, EmployeeID = employee.EmployeeID, Employee = employee };

                employee.Laptop = newLaptop;
            }
            else
            {
                newLaptop = new Laptop()
                {
                    LaptopID = laptopID,
                    LaptopName = Laptop.LaptopName
                };

            }

            _context.Laptop.Add(newLaptop);


            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLaptop), new { id = newLaptop.LaptopID }, newLaptop);
        }

        // DELETE: api/Laptops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaptop(int id)
        {
            var laptop = await _context.Laptop.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }

            _context.Laptop.Remove(laptop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptop.Any(e => e.LaptopID == id);
        }
    }
}
