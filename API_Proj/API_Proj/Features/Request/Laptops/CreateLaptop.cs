using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Laptops
{
    public static class CreateLaptop
    {
        public class Query: IRequest<ActionResult<LaptopDTO>>
        {
            public LaptopForCreationDTO _laptop;

            public Query(LaptopForCreationDTO laptop)
            {
                _laptop = laptop;
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
                if (request._laptop == null)
                {
                    return new BadRequestObjectResult("Laptop can't be null");
                }

                var Laptop = _mapper.Map<Laptop>(request._laptop);

                if (Laptop == null)
                {
                    return new NotFoundObjectResult("Laptop not found");
                }

                if (request._laptop.EmployeeID != null)
                {
                    var employee = await _context.Employee
                        .Include(e => e.Laptop)
                        .Include(e => e.Offices)
                        .Where(e => e.EmployeeID == request._laptop.EmployeeID).FirstOrDefaultAsync(cancellationToken);

                    if (employee == null)
                    {
                        return new NotFoundObjectResult("Employee not found");
                    }

                    if (employee.Laptop != null)
                    {
                        return new BadRequestObjectResult("Employee already has a laptop");
                    }

                    employee.Laptop = Laptop;
                }

                _context.Laptop.Add(Laptop);

                await _context.SaveChangesAsync(cancellationToken);


                var laptopDTO = _mapper.Map<LaptopDTO>(Laptop);

                return laptopDTO ?? new LaptopDTO();
            }

        }
    }
}
