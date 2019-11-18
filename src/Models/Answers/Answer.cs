using Festispec.Models.Questions;
using System.Collections.Generic;

namespace Festispec.Models.Answers
{
    public abstract class Answer : Entity
    {
        public int Id { get; set; }

        public virtual Question Question { get; set; }

        public virtual PlannedInspection PlannedInspection { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}