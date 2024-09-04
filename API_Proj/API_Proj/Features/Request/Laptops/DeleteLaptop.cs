using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Laptops {
    public static class DeleteLaptop {
        public class Query : IRequest<ActionResult<LaptopDTO>> {

            public int _laptopID;

            public Query(int laptopID) {
                _laptopID = laptopID;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<LaptopDTO>> {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<LaptopDTO>> Handle(Query request, CancellationToken cancellationToken) {

                var laptop = await _context.Laptop.FindAsync(request._laptopID);
                if (laptop == null) {
                    return new NotFoundObjectResult("Laptop doesn't exist");
                }


                laptop.IsDeleted = true;
                _context.Update(laptop);
                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}