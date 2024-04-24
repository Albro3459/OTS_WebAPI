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
        public async Task<ActionResult<LaptopDTO>> CreateLaptop([FromBody] LaptopForCreationDTO _Laptop)
        {

            if (_Laptop == null)
            {
                return BadRequest("Laptop can't be null");
            }

            var Laptop = _mapper.Map<Laptop>(_Laptop);

            if (_Laptop.EmployeeID != null)
            {
              
                var employee = await _context.Employee.Where(e => e.EmployeeID == _Laptop.EmployeeID).FirstOrDefaultAsync();
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

            var laptopDTO = await _context.Laptop
                .Where(l => l.LaptopID == Laptop.LaptopID)
                .Select(l => _mapper.Map<LaptopDTO>(l))
                .FirstOrDefaultAsync();

            return laptopDTO ?? new LaptopDTO();
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
