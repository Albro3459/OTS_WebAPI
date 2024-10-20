using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Employees
{
    public class UpdateEmployee
    {
        public class Query : IRequest<ActionResult<EmployeeDTO>>
        {

            public EmployeeDTO _employee;

            public Query(EmployeeDTO employee)
            {
                _employee = employee;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<EmployeeDTO>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<EmployeeDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request._employee == null)
                { return new BadRequestObjectResult("Employee can't be null"); }

                var oldEmployee = await _context.Employee
                    .Include(e => e.Laptop)
                    .Include(e => e.Offices)
                    .ThenInclude(o => o.Region)
                    .ThenInclude(r => r.Offices)
                    .Include(e => e.Offices)
                    .ThenInclude(o => o.Employees)
                    .Where(e => e.EmployeeID == request._employee.EmployeeID).FirstOrDefaultAsync(cancellationToken);

                if (oldEmployee == null)
                { return new NotFoundObjectResult("Employee doesn't exist"); }


                _mapper.Map(request._employee, oldEmployee);


                if (request._employee.OfficesIDs != null && request._employee.OfficesIDs.Count >= 0)
                {
                    oldEmployee.Offices.Clear();

                    foreach (var id in request._employee.OfficesIDs)
                    {
                        var office = await _context.Office.Where(o => o.OfficeID == id).FirstOrDefaultAsync(cancellationToken);

                        if (office == null) { 
                            return new NotFoundObjectResult("Office " + id + " doesn't exist"); 
                        }
                        oldEmployee.Offices.Add(office);
                    }

                }

                if (request._employee.LaptopID != null)
                {
                    if (oldEmployee.Laptop == null || oldEmployee.Laptop.LaptopID != request._employee.LaptopID)
                    {

                        var laptop = await _context.Laptop.Where(l => l.LaptopID == request._employee.LaptopID).FirstOrDefaultAsync(cancellationToken);

                        if (laptop == null) { return new NotFoundObjectResult("Laptop " + request._employee.LaptopID + " doesn't exist"); }

                        oldEmployee.Laptop = laptop;
                    }

                }

                _context.Update(oldEmployee);
                await _context.SaveChangesAsync(cancellationToken);

                var employeeDTO = _mapper.Map<EmployeeDTO>(oldEmployee);

                return employeeDTO;
            }
        }
    }
}
