using AutoMapper;
using API_Proj.Domain.Entity;
using API_Proj.Features.DTO;
using API_Proj.Features.Controllers;
using Microsoft.EntityFrameworkCore;

namespace API_Proj.Features;
public class Map: Profile
{
    public Map()
    {

        // map to model from DTO
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


        // map back from DTO to Model

        CreateMap<RegionDTO, Region>();

        CreateMap<OfficeDTO, Office>();

        CreateMap<EmployeeDTO, Employee>();

        CreateMap<LaptopDTO, Laptop>();


        // map back from CreationDTO to Model

        CreateMap<RegionForCreationDTO, Region>();

        CreateMap<OfficeForCreationDTO, Office>();

        CreateMap<EmployeeForCreationDTO, Employee>();

        CreateMap<LaptopForCreationDTO, Laptop>();


        //Update models

        CreateMap<Region, Region>();

        CreateMap<Office, Office>();

        CreateMap<Employee, Employee>();

        CreateMap<Laptop, Laptop>();

    }
}
