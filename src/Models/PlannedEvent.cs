using System;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class PlannedEvent : Entity
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Required, MaxLength(45)]
        public string EventTitle { get; set; }

        [Required]
        public virtual Employee Employee { get; set; }
    }
}