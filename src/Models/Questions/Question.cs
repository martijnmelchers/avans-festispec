using System.Collections.Generic;
using Festispec.Models.Answers;

namespace Festispec.Models.Questions
{
    public abstract class Question : Entity
    {
        public int Id { get; set; }

        public string Contents { get; set; }

        public virtual QuestionCategory Category { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public abstract GraphType GraphType { get; }
    }
}