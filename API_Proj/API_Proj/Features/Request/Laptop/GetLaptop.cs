using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API_Proj.Features.Request.Laptop
{
    public static class GetLaptop
    {
        public class Query : IRequest<ActionResult<IEnumerable<LaptopDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, <ActionResult<IEnumerable<LaptopDTO>>>>

            
        {
            public Handler(ApiDbContext context, IMapper mapper)
            {

            }

            public async Task<ActionResult<IEnumerable<LaptopDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {

            }

        }
    }
}
