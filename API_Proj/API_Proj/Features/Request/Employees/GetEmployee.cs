using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Employees
{
    public static class GetEmployee
    {
        public class Query : IRequest<ActionResult<IEnumerable<EmployeeDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, ActionResult<IEnumerable<EmployeeDTO>>>
        {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<IEnumerable<EmployeeDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employees = await _context.Employee
                    .Include(e => e.Offices)
                    .Include(e => e.Laptop)
                    .Select(e => _mapper.Map<EmployeeDTO>(e))
                    .ToListAsync(cancellationToken);

                return employees;
            }

        }
    }
}
