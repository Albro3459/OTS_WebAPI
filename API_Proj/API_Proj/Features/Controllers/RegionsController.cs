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
        private readonly IMediator _mediator;

        public RegionsController(IMediator mediator) { 
            _mediator = mediator;
        }

        // GET: api/Regions/Get
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

        // POST: api/Regions/Create
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

         // DELETE: api/Regions/Delete/5
        [HttpDelete("Delete/{regionID}")]
        public async Task<IActionResult> DeleteRegion(int regionID, CancellationToken cancellationToken) {
            var result = await _mediator.Send(new DeleteRegion.Query(regionID), cancellationToken);

            return result.Result ?? BadRequest();
        }
    }
}
