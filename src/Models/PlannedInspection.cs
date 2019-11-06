using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class PlannedInspection
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int WorkedHours { get; set; }

        public DateTime WorkedHoursAccepted { get; set; }

        public string CancellationReason { get; set; }

        public DateTime IsCancelled { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual Festival Festival { get; set; }

        public virtual Employee Inspector { get; set; }
    }
}