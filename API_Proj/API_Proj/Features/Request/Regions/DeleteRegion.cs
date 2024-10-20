using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace API_Proj.Features.Request.Regions {
    public class DeleteRegion {

        public class Query : IRequest<ActionResult<EmployeeDTO>> {

            public int _regionID;

            public Query(int regionID) {
                _regionID = regionID;
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
                var region = await _context.Region.FindAsync(request._regionID);
                if (region == null) {
                    return new NotFoundObjectResult("Region doesn't exist");
                }

                region.IsDeleted = true;

                var offices = await _context.Office.Where(o => o.RegionID == request._regionID).ToListAsync(cancellationToken);
                if (offices != null) {
                    foreach (var o in offices) {
                        o.Region = null;
                        o.RegionID = null;
                    }
                }

                //_context.Update(region);

                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}