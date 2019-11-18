using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Festispec.Models.Answers;

namespace Festispec.Models
{
    public class PlannedInspection : PlannedEvent
    {
        public int WorkedHours { get; set; }

        public DateTime WorkedHoursAccepted { get; set; }

        public string CancellationReason { get; set; }

        public DateTime IsCancelled { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual Festival Festival { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}