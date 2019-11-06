using System.Collections.Generic;

namespace Festispec.Models
{
    public abstract class Question
    {
        public int Id { get; set; }

        public string Contents { get; set; }

        public virtual QuestionCategory Category { get; set; }

        public virtual ICollection<Questionnaire> Questionnaires { get; set; }

        public abstract GraphType GraphType { get; }
    }
}