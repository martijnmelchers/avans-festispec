using System;
using System.Collections.Generic;

namespace Festispec.Models
{
    public abstract class Answer : Entity
    {
        public int Id { get; set; }

        public string Contents { get; set; }

        public virtual Employee Inspector { get; set; }

        public virtual Question Question { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}