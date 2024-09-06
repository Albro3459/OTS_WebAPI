using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Offices {
    public static class GetOfficeByID {
        public class Query : IRequest<ActionResult<OfficeDTO>> {
            public int _id;

            public Query(int id) {
                _id = id;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<OfficeDTO>> {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<OfficeDTO>> Handle(Query request, CancellationToken cancellationToken) {
                var office = await _context.Office
                    .Include(o => o.Employees)
                    .Include(o => o.Region)
                    .Where(o => o.OfficeID == request._id)
                    .Select(o => _mapper.Map<OfficeDTO>(o))
                    .SingleOrDefaultAsync(cancellationToken);

                if (office == null) {
                    return new NotFoundObjectResult("Office not found");
                }

                return office;
            }

        }
    }
}
