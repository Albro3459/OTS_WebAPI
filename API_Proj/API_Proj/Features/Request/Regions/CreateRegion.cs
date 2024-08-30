using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Regions
{
    public class CreateRegion
    {
        public class Query : IRequest<ActionResult<RegionDTO>>
        {
            public RegionForCreationDTO _region;

            public Query(RegionForCreationDTO region)
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
                {
                    return new BadRequestObjectResult("Region can't be null");
                }

                var Region = _mapper.Map<Region>(request._region);

                if (request._region.OfficesIDs != null && request._region.OfficesIDs.Count != 0)
                {
                    foreach (var id in request._region.OfficesIDs)
                    {
                        var office = await _context.Office
                            .Include(o => o.Employees)
                            .ThenInclude(e => e.Offices)
                            .ThenInclude(o => o.Region)
                            .Include(o => o.Employees)
                            .ThenInclude(e => e.Laptop)
                            .Include(o => o.Region)
                            .ThenInclude(r => r.Offices)
                            .Where(o => o.OfficeID == id).FirstOrDefaultAsync(cancellationToken);

                        if (office == null)
                        {
                            return new NotFoundObjectResult("Employees doesn't exist");
                        }

                        if (office.Region != null)
                        {
                            return new BadRequestObjectResult("Employee already works at an office");
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
                await _context.SaveChangesAsync(cancellationToken);

                //var regionDTO = await _context.Region
                //    .Where(r => r.RegionID == Region.RegionID)
                //    .Select(r => _mapper.Map<RegionDTO>(r))
                //    .FirstOrDefaultAsync();

                var regionDTO = _mapper.Map<RegionDTO>(Region);

                return regionDTO ?? new RegionDTO();
            }
        }
    }
}
