using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Questionnaire : Entity
    {
        public int Id { get; set; }

        public DateTime? IsComplete { get; set; }

        [Required]
        public virtual Festival Festival { get; set; }

        [Required]
        public virtual PlannedInspection PlannedInspection { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}