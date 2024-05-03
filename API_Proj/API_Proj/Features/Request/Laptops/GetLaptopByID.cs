using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Laptops
{
    public static class GetLaptopByID
    {
        public class Query: IRequest<ActionResult<LaptopDTO>>
        {
            public int _id;

            public Query(int id)
            {
                _id = id;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<LaptopDTO>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;
            

            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<LaptopDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var laptop = await _context.Laptop
                    .Where(l => l.LaptopID == request._id)
                    .Select(l => _mapper.Map<LaptopDTO>(l))
                    .SingleOrDefaultAsync(cancellationToken);

                return laptop;
            }

        }
    }
}
