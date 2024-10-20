using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Regions {
    public static class GetRegionByID {
        public class Query : IRequest<ActionResult<RegionDTO>> {
            public int _id;

            public Query(int id) {
                _id = id;
            }
        }

        public class Handler : IRequestHandler<Query, ActionResult<RegionDTO>> {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<RegionDTO>> Handle(Query request, CancellationToken cancellationToken) {
                var region = await _context.Region
                    .Include(r => r.Offices)
                    .ThenInclude(o => o.Employees)
                    .ThenInclude(e => e.Laptop)
                    .Where(r => r.RegionID == request._id)
                    .Select(r => _mapper.Map<RegionDTO>(r))
                    .SingleOrDefaultAsync(cancellationToken);

                if (region == null) {
                    return new NotFoundObjectResult("Region not found");
                }

                return region;
            }

        }
    }
}
