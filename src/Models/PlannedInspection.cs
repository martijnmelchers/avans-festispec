using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Festispec.Models.Answers;

namespace Festispec.Models
{
    public class PlannedInspection : PlannedEvent
    {
        public int WorkedHours { get; set; }

        public DateTime? WorkedHoursAccepted { get; set; }

        [MaxLength(250)] public string CancellationReason { get; set; }

        public DateTime? IsCancelled { get; set; }

        [Required] public virtual Questionnaire Questionnaire { get; set; }

        [Required] public virtual Festival Festival { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}