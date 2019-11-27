﻿using Festispec.Models.Questions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Answers
{
    public abstract class Answer : Entity
    {
        public int Id { get; set; }

        [Required]
        public virtual Question Question { get; set; }

        [Required]
        public virtual PlannedInspection PlannedInspection { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}