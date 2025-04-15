using System;
using System.ComponentModel.DataAnnotations;
using ProjectMap.WebApi.Models;

namespace ProjectMapGroepsproject.WebApi.Models
{
    public class Dagboek
    {
        public Guid Id { get; set; }

        [StringLength(250)]
        public string? DagboekBladzijde1 { get; set; }

        [StringLength(250)]
        public string? DagboekBladzijde2 { get; set; }

        [StringLength(250)]
        public string? DagboekBladzijde3 { get; set; }

        [StringLength(250)]
        public string? DagboekBladzijde4 { get; set; }

        public Guid ProfielKeuzeId { get; set; } // Foreign key to Profielkeuze

    }
}
