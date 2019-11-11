using System.Collections.Generic;

namespace Festispec.Models.Questions
{
    public abstract class Question : Entity
    {
        public int Id { get; set; }

        public string Contents { get; set; }

        public virtual QuestionCategory Category { get; set; }

        public virtual ICollection<Questionnaire> Questionnaires { get; set; }

        public abstract GraphType GraphType { get; }
    }
}