using API_Proj.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace API_Proj.Features.DTO
{
    public class EmployeeDTO
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        public string? JobTitle { get; set; }

        public double? YearsAtCompany { get; set; }

        public List<string>? CurrentProjects { get; set; } = new();

        public List<int>? OfficesIDs { get; set; } = new List<int>();

        public int? LaptopID { get; set; }

    }

    public class EmployeeForCreationDTO
    {
        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        public string? JobTitle { get; set; }

        public double? YearsAtCompany { get; set; }

        public List<string>? CurrentProjects { get; set; } = new();

        public List<int>? OfficesIDs { get; set; } = new List<int>();

        public int? LaptopID { get; set; }

    }
}
