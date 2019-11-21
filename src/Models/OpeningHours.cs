using System;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class OpeningHours
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}