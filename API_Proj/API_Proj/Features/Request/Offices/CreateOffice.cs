using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Offices
{
    public class CreateOffice
    {
        public class Query : IRequest<ActionResult<OfficeDTO>>
        {
            public OfficeForCreationDTO _office;

            public Query(OfficeForCreationDTO office)
            {
                _office = office;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<OfficeDTO>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<OfficeDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request._office == null)
                {
                    return new BadRequestObjectResult("Office can't be null");
                }

                var Office = _mapper.Map<Office>(request._office);

                if (Office == null)
                {
                    return new NotFoundObjectResult("Office not found");
                }

                if (request._office.EmployeesIDs != null && request._office.EmployeesIDs.Count != 0)
                {
                    foreach (var id in request._office.EmployeesIDs)
                    {
                        var employee = await _context.Employee
                            .Include(e => e.Offices)
                            .ThenInclude(o => o.Region)
                            .Include(e => e.Offices)
                            .ThenInclude(o => o.Employees)
                            .Include(e => e.Laptop)
                            .Where(e => e.EmployeeID == id).FirstOrDefaultAsync(cancellationToken);

                        if (employee == null)
                        {
                            return new NotFoundObjectResult("Employees doesn't exist");
                        }

                        if (employee.Offices != null && employee.Offices.Contains(Office))
                        {
                            continue;
                            //return BadRequest("Employee already works at that office");
                        }

                        Office.Employees.Add(employee);

                    }
                }

                if (request._office.RegionID != null)
                {
                    var region = await _context.Region.Where(r => r.RegionID == request._office.RegionID).FirstOrDefaultAsync(cancellationToken);
                    if (region == null)
                    {
                        return new NotFoundObjectResult("Region doesn't exist");
                    }
                    if (!region.Offices.Contains(Office))
                    {
                        Office.Region = region;
                    }

                    region.Offices.Add(Office);
                }

                // Uneccesary but works, ef core does it for me
                //foreach (var e in Office.Employees)
                //{
                //    e.Offices.Add(Office);
                //}

                _context.Office.Add(Office);
                await _context.SaveChangesAsync(cancellationToken);

                var officeDTO = _mapper.Map<OfficeDTO>(Office);

                return officeDTO ?? new OfficeDTO();
            }
        }
    }
}
