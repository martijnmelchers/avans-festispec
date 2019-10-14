using System;

namespace Festispec.Models
{
    public class OpeningHours
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public virtual Festival Festival { get; set; }
    }
}