using System;
using System.Collections.Generic;
using Festispec.Models.Answers;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public abstract class Question : Entity
    {
        public Question(string contents, Questionnaire questionnaire)
        {
            Contents = contents;
            Questionnaire = questionnaire;
            Answers = new List<Answer>();
        }
        public Question() { }

        public int Id { get; set; }

        [Required, MinLength(5), MaxLength(250)]
        public string Contents { get; set; }

        public virtual QuestionCategory Category { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public abstract GraphType GraphType { get; }

    }
}