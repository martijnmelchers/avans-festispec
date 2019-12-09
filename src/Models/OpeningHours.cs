using System;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class OpeningHours : Validateable
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}