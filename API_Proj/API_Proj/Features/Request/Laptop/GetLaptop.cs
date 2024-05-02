using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Laptop
{
    public static class GetLaptop
    {
        public class Query : IRequest<ActionResult<IEnumerable<LaptopDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, ActionResult<IEnumerable<LaptopDTO>>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;
            

            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<IEnumerable<LaptopDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var laptops = await _context.Laptop
                .Select(l => _mapper.Map<LaptopDTO>(l)).ToListAsync(cancellationToken);

                return laptops;
            }

        }
    }
}
