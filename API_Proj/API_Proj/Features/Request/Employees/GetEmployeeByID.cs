using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Employees {
    public static class GetEmployeeByID {
        public class Query : IRequest<ActionResult<EmployeeDTO>> {
            public int _id;

            public Query(int id) {
                _id = id;
            }

        }

        public class Handler : IRequestHandler<Query, ActionResult<EmployeeDTO>> {
            private readonly ApiDbContext _context;
            private readonly IMapper _mapper;


            public Handler(ApiDbContext context, IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<EmployeeDTO>> Handle(Query request, CancellationToken cancellationToken) {
                var employee = await _context.Employee
                    .Include(e => e.Offices)
                    .Include(e => e.Laptop)
                    .Where(e => e.EmployeeID == request._id)
                    .Select(e => _mapper.Map<EmployeeDTO>(e))
                    .SingleOrDefaultAsync(cancellationToken);

                if (employee == null) {
                    return new NotFoundObjectResult("Employee not found");
                }

                return employee;
            }

        }
    }
}
