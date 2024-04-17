using System.ComponentModel.DataAnnotations;

namespace API_Proj.Domain.Entity
{
    public class Employee
    {
        [Key]
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? JobTitle { get; set; }

        public double? YearsAtCompany { get; set; }

        public List<string>? CurrentProjects { get; set; } = new();

        public ICollection<Office> Offices { get; set; } = new List<Office>();

        public Laptop? Laptop { get; set; }

    }
}
