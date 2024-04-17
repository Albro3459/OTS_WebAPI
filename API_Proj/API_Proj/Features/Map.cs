using AutoMapper;
using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;

namespace API_Proj.Features;
public class Map: Profile
{
    public Map() {

        CreateMap<Region, RegionDTO>()
            .ForMember(dto => dto.OfficesIDs,
                        opt => opt.MapFrom(model => model.Offices.Select(o => o.OfficeID).ToList()));

        CreateMap<Office, OfficeDTO>()
            .ForMember(dto => dto.EmployeesIDs,
                        opt => opt.MapFrom(model => model.Employees.Select(o => o.EmployeeID).ToList()));

        CreateMap<Employee, EmployeeDTO>()
            .ForMember(dto => dto.OfficesIDs,
                        opt => opt.MapFrom(model => model.Offices.Select(o => o.OfficeID).ToList()))
            .ForMember(dto => dto.LaptopID, 
                        opt => opt.MapFrom(model => model.Laptop.LaptopID));

        CreateMap<Laptop, LaptopDTO>();
    }
}
