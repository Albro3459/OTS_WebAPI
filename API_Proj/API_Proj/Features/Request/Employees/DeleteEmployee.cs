using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Infastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features.Request.Employees {
    public class DeleteEmployee {
        public class Query : IRequest<ActionResult<EmployeeDTO>> {

            public int _employeeID;

            public Query(int employeeID) {
                _employeeID = employeeID;
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

                var employee = await _context.Employee.FindAsync(request._employeeID);
                if (employee == null || employee.IsDeleted) {
                    return new NotFoundObjectResult("Employee doesn't exist");
                }


                employee.IsDeleted = true;
                var laptop = await _context.Laptop.Where(l => l.EmployeeID == request._employeeID).FirstOrDefaultAsync(cancellationToken);
                if (laptop != null) {
                    laptop.Employee = null;
                    laptop.EmployeeID = null;
                }

                //_context.Update(employee);


                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();

            }
        }
    }
}