using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Proj.Domain.Entity
{
    public class Laptop
    {
        [Key]
        [Required]
        public int LaptopID { get; set; }

        [Required]
        [MaxLength(50)]
        public string LaptopName { get; set; } = string.Empty;

        [ForeignKey("EmployeeID")]
        public int? EmployeeID { get; set; }

        public Employee? Employee { get; set; } = null!;

    }
}
