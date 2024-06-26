﻿using API_Proj.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace API_Proj.Features.DTO
{
    public class RegionDTO
    {
        [Required]
        public int RegionID { get; set; }

        public string? RegionName { get; set; }

        public List<int>? OfficesIDs { get; set; }

    }

    public class RegionForCreationDTO
    {
        [Required]
        public string RegionName { get; set; } = string.Empty;

        public List<int>? OfficesIDs { get; set; } = new List<int>();

    }
}
