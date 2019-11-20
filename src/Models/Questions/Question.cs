using System.Collections.Generic;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;

namespace Festispec.Models.Questions
{
    public abstract class Question : Entity, IAnswerable<Answer>
    {
        public int Id { get; set; }

        public string Contents { get; set; }

        public virtual QuestionCategory Category { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public abstract GraphType GraphType { get; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
