using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Region
{
    public static class GetRegion
    {
        public class Query : IRequest<ActionResult<IEnumerable<RegionDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, ActionResult<IEnumerable<RegionDTO>>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<IEnumerable<RegionDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var regions = await _context.Region
                    .Include(r => r.Offices)
                    .ThenInclude(o => o.Employees)
                    .ThenInclude(e => e.Laptop)
                    .Select(r => _mapper.Map<RegionDTO>(r))
                    .ToListAsync(cancellationToken);

                return regions;
            }

        }
    }
}
