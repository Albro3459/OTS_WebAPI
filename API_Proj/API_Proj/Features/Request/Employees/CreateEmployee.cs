using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Employees
{
    public static class CreateEmployee
    {
        public class Query : IRequest<ActionResult<EmployeeDTO>>
        {
            public EmployeeForCreationDTO _employee;

            public Query(EmployeeForCreationDTO employee)
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
                {
                    return new BadRequestObjectResult("Employee can't be null");
                }

                var Employee = _mapper.Map<Employee>(request._employee);

                if (Employee == null)
                {
                    return new NotFoundObjectResult("Employee not found");
                }

                if (request._employee.LaptopID != null)
                {

                    var laptop = await _context.Laptop
                        .Include(l => l.Employee)
                        .ThenInclude(e => e.Offices)
                        .ThenInclude(o => o.Region)
                        .Where(l => l.LaptopID == request._employee.LaptopID)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (laptop == null)
                    {
                        return new NotFoundObjectResult("Employee not found");
                    }

                    if (laptop.EmployeeID != null)
                    {
                        return new BadRequestObjectResult("Laptop already assigned to an employee");
                    }

                    Employee.Laptop = laptop;
                }

                if (request._employee.OfficesIDs != null && request._employee.OfficesIDs.Count != 0)
                {
                    var offices = new List<Office>();

                    foreach (var id in request._employee.OfficesIDs)
                    {

                        var office = await _context.Office
                            .Include(o => o.Employees)
                            .ThenInclude(e => e.Laptop)
                            .Include(o => o.Employees)
                            .ThenInclude(e => e.Offices)
                            .ThenInclude(o => o.Region)
                            .Include(o => o.Region)
                            .ThenInclude(r => r.Offices)
                            .Where(o => o.OfficeID == id).FirstOrDefaultAsync(cancellationToken);

                        if (office == null)
                        {
                            return new NotFoundObjectResult("Office not found");
                        }
                        office.Employees.Add(Employee);
                    }
                }

                _context.Employee.Add(Employee);

                await _context.SaveChangesAsync(cancellationToken);

                //var employeeDTO = await _context.Employee
                //    .Where(e => e.EmployeeID == Employee.EmployeeID)
                //    .Select(e => _mapper.Map<EmployeeDTO>(e))
                //    .FirstOrDefaultAsync();

                var employeeDTO = _mapper.Map<EmployeeDTO>(Employee);


                return employeeDTO ?? new EmployeeDTO();
            }

        }
    }
}
