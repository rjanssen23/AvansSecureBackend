using System;
using System.ComponentModel.DataAnnotations;
using ProjectMap.WebApi.Models;

namespace ProjectMapGroepsproject.WebApi.Models
{
    public class Agenda
    {
        public Guid Id { get; set; }

        [StringLength(10)]
        public string date1 { get; set; }

        [StringLength(50)]
        public string location1 { get; set; }

        [StringLength(10)]
        public string date2 { get; set; }

        [StringLength(50)]
        public string location2 { get; set; }

        [StringLength(10)]
        public string date3 { get; set; }

        [StringLength(50)]
        public string location3 { get; set; }

        public Guid ProfielKeuzeId { get; set; } // Foreign key to ProfielKeuze
    }
}
