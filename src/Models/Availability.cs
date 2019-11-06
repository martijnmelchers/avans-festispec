using System;

namespace Festispec.Models
{
    public class Availability
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsAvailable { get; set; }

        public string Reason { get; set; }

        public virtual Employee Employee { get; set; }
    }
}