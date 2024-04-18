﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")] // api/Regions
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public RegionsController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Regions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegion()
        {
            var regions = await _context.Region
                .Include(r => r.Offices)
                .Select(r => _mapper.Map<RegionDTO>(r))
                .ToListAsync();

            return regions;
        }

        // GET: api/Regions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegionDTO>> GetRegion(int id)
        {
            var region = await _context.Region
                .Include(r => r.Offices)
                .Where(r => r.RegionID == id)
                .Select(r => _mapper.Map<RegionDTO>(r))
                .SingleOrDefaultAsync();

            if (region == null)
            {
                return NotFound();
            }

            return region;
        }

        // PUT: api/Regions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegion(int id, RegionDTO regionDTO)
        {
            if (id != regionDTO.RegionID)
            {
                return BadRequest();
            }

            var region = _mapper.Map<Region>(regionDTO);

            //region.Offices.Select(o => OfficesController.GetOffice(o.OfficeID));

            _context.Entry(region).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegionExists(id))
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

        // POST: api/Regions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Region>> PostRegion(Region region)
        {
            _context.Region.Add(region);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegion", new { id = region.RegionID }, region);
        }

        // DELETE: api/Regions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var region = await _context.Region.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            _context.Region.Remove(region);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegionExists(int id)
        {
            return _context.Region.Any(e => e.RegionID == id);
        }
    }
}
