using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMap.WebApi.Models
{
    public class ProfielKeuze
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public string? Arts { get; set; }

        [Required]
        public int? GeboorteDatum { get; set; }

        [Required]
        public string? Avatar { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; } // Navigatie-eigenschap
    }
}
