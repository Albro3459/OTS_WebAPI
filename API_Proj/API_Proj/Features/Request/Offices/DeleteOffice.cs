﻿using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Offices {
    public class DeleteOffice {

        public class Query : IRequest<ActionResult<EmployeeDTO>> {

            public int _officeID;

            public Query(int officeID) {
                _officeID = officeID;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<EmployeeDTO>> {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<EmployeeDTO>> Handle(Query request, CancellationToken cancellationToken) {
                var office = await _context.Office.FindAsync(request._officeID);
                if (office == null) {
                    return new NotFoundObjectResult("Office doesn't exist");
                }

                office.IsDeleted = true;
                _context.Update(office);

                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}