using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Proj.Domain.Entity
{
    public class Office
    {
        [Key]
        [Required]
        public int OfficeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string OfficeName { get; set; } = string.Empty;

        [ForeignKey("RegionID")]
        public int? RegionID { get; set; }
        public Region? Region { get; set; } = null!;

        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
            
    }
}