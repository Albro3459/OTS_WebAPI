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
using API_Proj.Features.Request.Offices;
using System.Threading;
using API_Proj.Features.Request.Employees;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfficesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Offices/Get
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<OfficeDTO>>> GetOffice(CancellationToken cancellationToken)
        {
            var offices = await _mediator.Send(new GetOffice.Query(), cancellationToken);

            if (offices.Value != null)
            {
                return Ok(offices.Value);
            }

            return offices.Result ?? BadRequest();
        }


        // GET: api/Offices/Get/1001
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<OfficeDTO>> GetOfficeByID(int id, CancellationToken cancellationToken)
        {
            var office = await _mediator.Send(new GetOfficeByID.Query(id), cancellationToken);

            if (office.Value != null)
            {
                return Ok(office.Value);
            }

            return office.Result ?? BadRequest();
        }
        
        // PUT: api/Offices/Update/
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOffice(OfficeDTO _office, CancellationToken cancellationToken)
        {

            var office = await _mediator.Send(new UpdateOffice.Query(_office), cancellationToken);

            if (office.Value != null)
            {
                return Ok(office.Value);
            }

            return office.Result ?? BadRequest();
        }

        //PUT: api/Offices/Update/{_officeID}/UnassignOfficeRegion
        [HttpPut("Update/{_officeID}/UnassignOfficeRegion")]
        public async Task<IActionResult> UnassignOfficeRegion(int _officeID, CancellationToken cancellationToken)
        { 
            var office = await _mediator.Send(new UnassignOfficeRegion.Query(_officeID), cancellationToken);

            if (office.Value != null)
            {
                return Ok(office.Value);
            }

            return office.Result ?? BadRequest();
        }

        // POST: api/Offices/Create
        [HttpPost("Create")]
        public async Task<ActionResult<OfficeDTO>> CreateOffice([FromBody] OfficeForCreationDTO _office, CancellationToken cancellationToken)
        {
            var office = await _mediator.Send(new CreateOffice.Query(_office), cancellationToken);

            if (office.Value != null)
            {
                return Ok(office.Value);
            }

            return office.Result ?? BadRequest();

        }

        // DELETE: api/Offices/Delete/5
        [HttpDelete("Delete/{officeID}")]
        public async Task<IActionResult> DeleteOffice(int officeID, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteOffice.Query(officeID), cancellationToken);

            return result.Result ?? BadRequest();
        }
    }
}
