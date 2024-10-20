using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Laptops {
    public static class UnassignLaptopOwner {
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
                var laptop = await _context.Laptop
                .Include(l => l.Employee)
                .Where(l => l.LaptopID == request._laptopID).FirstOrDefaultAsync(cancellationToken);

                if (laptop == null) {
                    return new NotFoundObjectResult("Laptop doesn't exist");
                }

                if (laptop.EmployeeID == null || laptop.Employee == null) {
                    return new NotFoundObjectResult("Laptop doesn't have an employee");
                }

                var employee = await _context.Employee.Where(e => e.EmployeeID == laptop.EmployeeID).FirstOrDefaultAsync(cancellationToken);

                if (employee == null) {
                    return new NotFoundObjectResult("Employee doesn't exist");
                }

                laptop.Employee = null;
                laptop.EmployeeID = null;


                employee.Laptop = null;

                await _context.SaveChangesAsync(cancellationToken);

                var laptopDTO = _mapper.Map<LaptopDTO>(laptop);
                return laptopDTO;

            }
        }
    }
}