using API_Proj.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Proj.Features.DTO
{
    public class OfficeDTO
    {
        [Required]
        public int OfficeID { get; set; }

        public string? OfficeName { get; set; }

        public int? RegionID { get; set; }

        public List<int>? EmployeesIDs { get; set; } = new List<int>();

    }

    public class OfficeForCreationDTO
    {
        [Required]
        public string OfficeName { get; set; } = string.Empty;

        public int? RegionID { get; set; }

        public List<int>? EmployeesIDs { get; set; } = new List<int>();

    }
}
