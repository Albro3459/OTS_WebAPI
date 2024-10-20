using System;
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
using MediatR;
using API_Proj.Features.Request.Regions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Drawing;
using API_Proj.Features.Request.Offices;
using System.Threading;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")] // api/Regions
    [ApiController]
    public class RegionsController : ControllerBase
    {
        //private readonly ApiDbContext _context;
        //private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public RegionsController(/*ApiDbContext context, IMapper mapper,*/ IMediator mediator)
        {
            //_context = context;
            //_mapper = mapper;
            _mediator = mediator;
        }

        // GET: api/Regions/Get

        //public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegion()
        //{
        //    var regions = await _context.Region
        //        .Include(r => r.Offices)
        //        .ThenInclude(o => o.Employees)
        //        .ThenInclude(e => e.Laptop)
        //        .Select(r => _mapper.Map<RegionDTO>(r))
        //        .ToListAsync();

        //    return regions;
        //}
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegion(CancellationToken cancellationToken)
        {
            var regions = await _mediator.Send(new GetRegion.Query(), cancellationToken);

            if (regions.Value != null)
            {
                return Ok(regions.Value);
            }

            return regions.Result ?? BadRequest();
        }

        // GET: api/Regions/1001
        //[HttpGet("Get/{id}")]
        //public async Task<ActionResult<RegionDTO>> GetRegion(int id)
        //{
        //    var region = await _context.Region
        //        .Include(r => r.Offices)
        //        .ThenInclude(o => o.Employees)
        //        .ThenInclude(e => e.Laptop)
        //        .Where(r => r.RegionID == id)
        //        .Select(r => _mapper.Map<RegionDTO>(r))
        //        .SingleOrDefaultAsync();

        //    if (region == null)
        //    {
        //        return NotFound();
        //    }

        //    return region;
        //}

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<RegionDTO>> GetRegionByID(int id, CancellationToken cancellationToken)
        {
            var region = await _mediator.Send(new GetRegionByID.Query(id), cancellationToken);

            if (region.Value != null)
            {
                return Ok(region.Value);
            }

            return region.Result ?? BadRequest();
        }



        //// PUT: api/Regions/Update/
        //[HttpPut("Update/")]
        //public async Task<IActionResult> UpdateRegion(RegionDTO _region)
        //{
        //    if (_region == null)
        //    { return NotFound("Region can't be null"); }

        //    var oldRegion = await _context.Region
        //        .Include(r => r.Offices)
        //        .ThenInclude(o => o.Employees)
        //        .ThenInclude(e => e.Laptop)
        //        .Where(r => r.RegionID == _region.RegionID).FirstOrDefaultAsync();

        //    if (oldRegion == null)
        //    { return NotFound("Region doesn't exist"); }


        //    _mapper.Map(_region, oldRegion);


        //    if (_region.OfficesIDs != null && _region.OfficesIDs.Count >= 0)
        //    {
        //        oldRegion.Offices.Clear();

        //        foreach (var id in _region.OfficesIDs)
        //        {
        //            var office = await _context.Office.Where(o => o.OfficeID == id).FirstOrDefaultAsync();
        //            if (office == null) { continue; }
        //            oldRegion.Offices.Add(office);
        //        }

        //    }

        //    _context.Update(oldRegion);
        //    await _context.SaveChangesAsync();

        //    var regionDTO = _mapper.Map<RegionDTO>(oldRegion);

        //    return Ok(regionDTO);
        //}
        // PUT: api/Regions/Update/
        [HttpPut("Update/")]
        public async Task<IActionResult> UpdateRegion(RegionDTO _region, CancellationToken cancellationToken)
        {
            var region = await _mediator.Send(new UpdateRegion.Query(_region), cancellationToken);

            if (region.Value != null) {
                return Ok(region.Value);
            }

            return region.Result ?? BadRequest();
        }

        //// POST: api/Regions
        //[HttpPost("Create")]
        //public async Task<ActionResult<RegionDTO>> CreateRegion(RegionForCreationDTO _region)
        //{

        //    if (_region == null)
        //    {
        //        return NotFound("Region can't be null");
        //    }

        //    var Region = _mapper.Map<Region>(_region);

        //    if (_region.OfficesIDs != null && _region.OfficesIDs.Count != 0)
        //    {
        //        foreach (var id in _region.OfficesIDs)
        //        {
        //            var office = await _context.Office
        //                .Include(o => o.Employees)
        //                .ThenInclude(e => e.Offices)
        //                .ThenInclude(o => o.Region)
        //                .Include(o => o.Employees)
        //                .ThenInclude(e => e.Laptop)
        //                .Include(o => o.Region)
        //                .ThenInclude(r => r.Offices)
        //                .Where(o => o.OfficeID == id).FirstOrDefaultAsync();

        //            if (office == null)
        //            {
        //                return NotFound("Employees doesn't exist");
        //            }

        //            if (office.Region == Region)
        //            {
        //                continue;
        //                //return NotFound("Employee already works at that office");
        //            }
        //            Region.Offices.Add(office);

        //        }
        //    }

        //    _context.Region.Add(Region);
        //    await _context.SaveChangesAsync();

        //    //var regionDTO = await _context.Region
        //    //    .Where(r => r.RegionID == Region.RegionID)
        //    //    .Select(r => _mapper.Map<RegionDTO>(r))
        //    //    .FirstOrDefaultAsync();

        //    var regionDTO = _mapper.Map<RegionDTO>(Region);

        //    return regionDTO ?? new RegionDTO();
        //}

        // POST: api/Regions
        [HttpPost("Create")]
        public async Task<ActionResult<RegionDTO>> CreateRegion(RegionForCreationDTO _region, CancellationToken cancellationToken)
        {
            var region = await _mediator.Send(new CreateRegion.Query(_region), cancellationToken);

            if (region.Value != null)
            {
                return Ok(region.Value);
            }

            return region.Result ?? BadRequest();
        }


        //// DELETE: api/Regions/Delete/5
        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> DeleteRegion(int id)
        //{
        //    var region = await _context.Region.FindAsync(id);
        //    if (region == null)
        //    {
        //        return NotFound("Region doesn't exist");
        //    }

        //    region.IsDeleted = true;

        //    var offices = await _context.Office.Where(o => o.RegionID == id).ToListAsync();
        //    if (offices != null)
        //    {
        //        foreach (var o in offices)
        //        {
        //            o.Region = null;
        //            o.RegionID = null;
        //        }
        //    }

        //    //_context.Update(region);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        // DELETE: api/Regions/Delete/5
        [HttpDelete("Delete/{regionID}")]
        public async Task<IActionResult> DeleteRegion(int regionID, CancellationToken cancellationToken) {
            var result = await _mediator.Send(new DeleteRegion.Query(regionID), cancellationToken);

            return result.Result ?? BadRequest();
        }

        //private bool RegionExists(int id)
        //{
        //    return _context.Region.Any(e => e.RegionID == id);
        //}
    }
}
