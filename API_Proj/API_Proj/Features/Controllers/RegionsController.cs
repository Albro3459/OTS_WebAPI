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

        // GET: api/Regions/Get
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegion()
        {
            var regions = await _context.Region
                .Include(r => r.Offices)
                .ThenInclude(o => o.Employees)
                .ThenInclude(e => e.Laptop)
                .Select(r => _mapper.Map<RegionDTO>(r))
                .ToListAsync();

            return regions;
        }

        // GET: api/Regions/1001
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<RegionDTO>> GetRegion(int id)
        {
            var region = await _context.Region
                .Include(r => r.Offices)
                .ThenInclude(o => o.Employees)
                .ThenInclude(e => e.Laptop)
                .Where(r => r.RegionID == id)
                .Select(r => _mapper.Map<RegionDTO>(r))
                .SingleOrDefaultAsync();

            if (region == null)
            {
                return NotFound();
            }

            return region;
        }

        // PUT: api/Regions/Update/
        [HttpPut("Update/")]
        public async Task<IActionResult> UpdateRegion(RegionDTO _region)
        {
            if (_region == null)
            { return NotFound("Region can't be null"); }

            var oldRegion = await _context.Region
                .Include(r => r.Offices)
                .ThenInclude(o => o.Employees)
                .ThenInclude(e => e.Laptop)
                .Where(r => r.RegionID == _region.RegionID).FirstOrDefaultAsync();

            if (oldRegion == null)
            { return NotFound("Region doesn't exist"); }


            _mapper.Map(_region, oldRegion);


            if (_region.OfficesIDs != null && _region.OfficesIDs.Count >= 0)
            {
                oldRegion.Offices.Clear();

                foreach (var id in _region.OfficesIDs)
                {
                    var office = await _context.Office.Where(o => o.OfficeID == id).FirstOrDefaultAsync();
                    if (office == null) { continue; }
                    oldRegion.Offices.Add(office);
                }

            }

            _context.Update(oldRegion);
            await _context.SaveChangesAsync();

            var regionDTO = _mapper.Map<RegionDTO>(oldRegion);

            return Ok(regionDTO);
        }

        // POST: api/Regions
        [HttpPost("Create")]
        public async Task<ActionResult<RegionDTO>> CreateRegion(RegionForCreationDTO _region)
        {

            if (_region == null)
            {
                return NotFound("Region can't be null");
            }

            var Region = _mapper.Map<Region>(_region);

            if (_region.OfficesIDs != null && _region.OfficesIDs.Count != 0)
            {
                foreach (var id in _region.OfficesIDs)
                {
                    var office = await _context.Office
                        .Include(o => o.Employees)
                        .ThenInclude(e => e.Offices)
                        .ThenInclude(o => o.Region)
                        .Include(o => o.Employees)
                        .ThenInclude(e => e.Laptop)
                        .Include(o => o.Region)
                        .ThenInclude(r => r.Offices)
                        .Where(o => o.OfficeID == id).FirstOrDefaultAsync();

                    if (office == null)
                    {
                        return NotFound("Employees doesn't exist");
                    }

                    if (office.Region == Region)
                    {
                        continue;
                        //return NotFound("Employee already works at that office");
                    }
                    Region.Offices.Add(office);

                }
            }

            _context.Region.Add(Region);
            await _context.SaveChangesAsync();

            //var regionDTO = await _context.Region
            //    .Where(r => r.RegionID == Region.RegionID)
            //    .Select(r => _mapper.Map<RegionDTO>(r))
            //    .FirstOrDefaultAsync();

            var regionDTO = _mapper.Map<RegionDTO>(Region);

            return regionDTO ?? new RegionDTO();
        }


        // DELETE: api/Regions/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var region = await _context.Region.FindAsync(id);
            if (region == null)
            {
                return NotFound("Region doesn't exist");
            }

            region.IsDeleted = true;

            var offices = await _context.Office.Where(o => o.RegionID == id).ToListAsync();
            if (offices != null)
            {
                foreach (var o in offices)
                {
                    o.Region = null;
                    o.RegionID = null;
                }
            }

            //_context.Update(region);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegionExists(int id)
        {
            return _context.Region.Any(e => e.RegionID == id);
        }
    }
}
