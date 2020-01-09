using System;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class OpeningHours : Validateable
    {
        [Required] public TimeSpan StartTime { get; set; }

        [Required] public TimeSpan EndTime { get; set; }

        [Required] public DateTime StartDate { get; set; }

        [Required] public DateTime EndDate { get; set; }
    }
}