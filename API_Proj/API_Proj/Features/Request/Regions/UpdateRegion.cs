using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace API_Proj.Features.Request.Regions
{
    public class UpdateRegion
    {
        public class Query : IRequest<ActionResult<RegionDTO>>
        {

            public RegionDTO _region;

            public Query(RegionDTO region)
            {
                _region = region;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<RegionDTO>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<RegionDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request._region == null)
                { return new BadRequestObjectResult("Region can't be null"); }

                var oldRegion = await _context.Region
                    .Include(r => r.Offices)
                    //.ThenInclude(o => o.Employees)
                    //.ThenInclude(e => e.Laptop)
                    .Where(r => r.RegionID == request._region.RegionID).FirstOrDefaultAsync(cancellationToken);

                if (oldRegion == null)
                { return new NotFoundObjectResult("Region doesn't exist"); }


                _mapper.Map(request._region, oldRegion);


                if (request._region.OfficesIDs != null && request._region.OfficesIDs.Count >= 0)
                {
                    oldRegion.Offices.Clear();

                    foreach (var id in request._region.OfficesIDs)
                    {
                        var office = await _context.Office.Where(o => o.OfficeID == id).FirstOrDefaultAsync(cancellationToken);
                        if (office == null) {
                            return new NotFoundObjectResult("Office " + id + " doesn't exist");
                        }
                        oldRegion.Offices.Add(office);
                    }

                }

                _context.Update(oldRegion);
                await _context.SaveChangesAsync(cancellationToken);

                var regionDTO = _mapper.Map<RegionDTO>(oldRegion);

                return regionDTO;
            }
        }
    }
}
