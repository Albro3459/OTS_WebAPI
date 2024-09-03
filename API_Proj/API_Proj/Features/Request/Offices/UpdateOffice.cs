using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Offices
{
    public static class UpdateOffice
    {

        public class Query : IRequest<ActionResult<OfficeDTO>>
        {

            public OfficeDTO _office;

            public Query(OfficeDTO office)
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
                { return new BadRequestObjectResult("Office can't be null"); }

                var oldOffice = await _context.Office
                    .Include(o => o.Employees)
                    .ThenInclude(e => e.Laptop)
                    .Include(o => o.Employees)
                    .ThenInclude(e => e.Offices)
                    .ThenInclude(o => o.Region)
                    .Include(o => o.Region)
                    .ThenInclude(r => r.Offices)
                    .ThenInclude(o => o.Employees)
                    .Where(o => o.OfficeID == request._office.OfficeID).FirstOrDefaultAsync(cancellationToken);

                if (oldOffice == null)
                { return new NotFoundObjectResult("Office doesn't exist"); }


                _mapper.Map(request._office, oldOffice);


                if (request._office.EmployeesIDs != null && request._office.EmployeesIDs.Count >= 0)
                {
                    oldOffice.Employees.Clear();

                    foreach (var id in request._office.EmployeesIDs)
                    {
                        var employee = await _context.Employee.Where(e => e.EmployeeID == id).FirstOrDefaultAsync(cancellationToken);
                        if (employee == null) {
                            return new NotFoundObjectResult("Employee " + id + " doesn't exist");
                        }
                        oldOffice.Employees.Add(employee);
                    }

                }

                if (request._office.RegionID != null)
                {
                    if (oldOffice.Region == null || oldOffice.Region.RegionID != request._office.RegionID)
                    {
                        var region = await _context.Region.Where(r => r.RegionID == request._office.RegionID).FirstOrDefaultAsync(cancellationToken);

                        if (region == null) { return new NotFoundObjectResult("Region " + request._office.RegionID + " doesn't exist"); }

                        oldOffice.Region = region;
                    }

                }

                _context.Update(oldOffice);
                await _context.SaveChangesAsync(cancellationToken);

                var officeDTO = _mapper.Map<OfficeDTO>(oldOffice);

                return officeDTO;

            }
        }
    }

    public static class UnassignRegion
    {

        public class Query : IRequest<ActionResult<OfficeDTO>>
        {

            public int _officeID;

            public Query(int officeID)
            {
                _officeID = officeID;
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
                var office = await _context.Office
                .Include(o => o.Employees)
                    .ThenInclude(e => e.Laptop)
                .Include(o => o.Employees)
                    .ThenInclude(e => e.Offices)
                        .ThenInclude(o => o.Region)
                .Include(o => o.Region)
                    .ThenInclude(r => r.Offices)
                .Where(o => o.OfficeID == request._officeID).FirstOrDefaultAsync(cancellationToken);

                if (office == null)
                {
                    return new BadRequestObjectResult("Office doesn't exist");
                }

                if (office.RegionID == null || office.Region == null)
                {
                    return new NotFoundObjectResult("Office doesn't have an region");
                }

                var region = await _context.Region.Where(r => r.RegionID == office.RegionID).FirstOrDefaultAsync(cancellationToken);

                if (region == null)
                {
                    return new NotFoundObjectResult("Region doesn't exist");
                }

                office.Region = null;
                office.RegionID = null;

                if (region.Offices != null && region.Offices.Count > 0)
                {
                    region.Offices.Remove(office);
                }


                await _context.SaveChangesAsync(cancellationToken);

                var officeDTO = _mapper.Map<OfficeDTO>(office);

                return officeDTO;
            }
        }
    }
}