using API_Proj.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Proj.Features.DTO
{
    public class LaptopDTO
    {
        [Required]
        public int LaptopID { get; set; }

        [Required]
        public string LaptopName { get; set; } = string.Empty;

        public int? EmployeeID { get; set; }

    }
}
