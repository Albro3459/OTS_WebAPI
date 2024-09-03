using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Laptops
{
    public static class UpdateLaptop
    {
        public class Query : IRequest<ActionResult<LaptopDTO>>
        {

            public LaptopDTO _laptop;

            public Query(LaptopDTO laptop)
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

                var oldLaptop = await _context.Laptop
                    .Include(l => l.Employee)
                    .Where(l => l.LaptopID == request._laptop.LaptopID).FirstOrDefaultAsync(cancellationToken);

                if (oldLaptop == null)
                {
                    return new NotFoundObjectResult("Laptop doesn't exist");
                }

                _mapper.Map(request._laptop, oldLaptop);

                if (request._laptop.EmployeeID != null)
                {
                    if (oldLaptop.Employee == null || oldLaptop.Employee.EmployeeID != request._laptop.EmployeeID)
                    {
                        var employee = await _context.Employee
                            .Include(e => e.Laptop)
                            .Include(e => e.Offices)
                            .ThenInclude(o => o.Region)
                            .Include(e => e.Offices)
                            .ThenInclude(o => o.Employees)
                            .ThenInclude(e => e.Laptop)
                            .Where(e => e.EmployeeID == request._laptop.EmployeeID).FirstOrDefaultAsync(cancellationToken);

                        if (employee == null) { return new NotFoundObjectResult("Employee " + request._laptop.EmployeeID + " doesn't exist"); }

                        oldLaptop.EmployeeID = request._laptop.EmployeeID;
                        oldLaptop.Employee = employee;
                    }

                }

                _context.Update(oldLaptop);
                await _context.SaveChangesAsync(cancellationToken);

                var returnLaptop = _mapper.Map<LaptopDTO>(oldLaptop);

                return returnLaptop;
            }
        }
    }

    public static class UnassignOwner
    {
        public class Query : IRequest<ActionResult<LaptopDTO>>
        {

            public int _laptopID;

            public Query(int laptopID)
            {
                _laptopID = laptopID;
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
                .Include(l => l.Employee)
                .Where(l => l.LaptopID == request._laptopID).FirstOrDefaultAsync(cancellationToken);

                if (laptop == null)
                {
                    return new NotFoundObjectResult("Laptop doesn't exist");
                }

                if (laptop.EmployeeID == null || laptop.Employee == null)
                {
                    return new NotFoundObjectResult("Laptop doesn't have an employee");
                }

                var employee = await _context.Employee.Where(e => e.EmployeeID == laptop.EmployeeID).FirstOrDefaultAsync(cancellationToken);

                if (employee == null)
                {
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