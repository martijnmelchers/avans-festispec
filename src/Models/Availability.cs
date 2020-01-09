using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Availability : PlannedEvent
    {
        [Required] public bool IsAvailable { get; set; }

        [MaxLength(250)] public string Reason { get; set; }
    }
}