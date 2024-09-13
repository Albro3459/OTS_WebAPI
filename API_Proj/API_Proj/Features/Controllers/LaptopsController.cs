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
using System.Threading;

namespace API_Proj.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LaptopsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Laptops/Get
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<LaptopDTO>>> GetLaptop(CancellationToken cancellationToken)
        {
            var laptops = await _mediator.Send(new GetLaptop.Query(), cancellationToken);

            if (laptops.Value != null) {
                return Ok(laptops.Value);
            }

            return laptops.Result ?? BadRequest();
        }


        // GET: api/Laptops/Get/1001
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<LaptopDTO>> GetLaptopByID(int id, CancellationToken cancellationToken)
        {
            var laptop = await _mediator.Send(new GetLaptopByID.Query(id), cancellationToken);

            if (laptop.Value != null) {
                return Ok(laptop.Value);
            }

            return laptop.Result ?? BadRequest();
        }


        //PUT: api/Laptops/Update
        [HttpPut("Update/")]
        public async Task<IActionResult> UpdateLaptop(LaptopDTO _laptop, CancellationToken cancellationToken)
        {
            var laptop = await _mediator.Send(new UpdateLaptop.Query(_laptop), cancellationToken);

            if (laptop.Value != null) {
                return Ok(laptop.Value);
            }

            return laptop.Result ?? BadRequest();

        }

        //PUT: api/Laptops/Update/{_laptopID}/UnassignLaptopOwner
        [HttpPut("Update/{_laptopID}/UnassignLaptopOwner")]
        public async Task<IActionResult> UnassignLaptopOwner(int _laptopID, CancellationToken cancellationToken)
        {
            var laptop = await _mediator.Send(new UnassignLaptopOwner.Query(_laptopID), cancellationToken);

            if (laptop.Value != null) {
                return Ok(laptop.Value);
            }

            return laptop.Result ?? BadRequest();

        }

        // POST: api/Laptops/Create
        [HttpPost("Create")]
        public async Task<ActionResult<LaptopDTO>> CreateLaptop([FromBody] LaptopForCreationDTO _laptop, CancellationToken cancellationToken)
        {
            var laptop = await _mediator.Send(new CreateLaptop.Query(_laptop), cancellationToken);

            if (laptop.Value != null)
            {
                return Ok(laptop.Value);
            }

            return laptop.Result ?? BadRequest();
        }

        // DELETE: api/Laptops/Delete/5
        [HttpDelete("Delete/{laptopID}")]
        public async Task<IActionResult> DeleteLaptop(int laptopID, CancellationToken cancellationToken) {

            var result = await _mediator.Send(new DeleteLaptop.Query(laptopID), cancellationToken);

            return result.Result ?? BadRequest();
        }
    }
}
