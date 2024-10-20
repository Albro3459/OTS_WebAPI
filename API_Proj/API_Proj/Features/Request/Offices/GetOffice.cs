using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Offices
{
    public static class GetOffice
    {
        public class Query : IRequest<ActionResult<IEnumerable<OfficeDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, ActionResult<IEnumerable<OfficeDTO>>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<IEnumerable<OfficeDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var offices = await _context.Office
                    .Include(o => o.Employees).ThenInclude(e => e.Laptop)
                    .Include(o => o.Region)
                    .Select(o => _mapper.Map<OfficeDTO>(o))
                    .ToListAsync(cancellationToken);

                return offices;
            }

        }
    }
}
