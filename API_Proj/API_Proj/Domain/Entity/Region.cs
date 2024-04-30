using System.ComponentModel.DataAnnotations;

namespace API_Proj.Domain.Entity
{
    public class Region
    {
        [Key]
        [Required]
        public int RegionID { get; set; }

        [Required]
        [MaxLength(50)]
        public string RegionName { get; set; } = string.Empty;

        public ICollection<Office>? Offices { get; set; } = new List<Office>();

        public bool IsDeleted { get; set; } = false;
    }
}
