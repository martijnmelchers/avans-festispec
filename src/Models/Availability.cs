using System;

namespace Festispec.Models
{
    public class Availability : PlannedEvent
    {
        public bool IsAvailable { get; set; }

        public string Reason { get; set; }
    }
}