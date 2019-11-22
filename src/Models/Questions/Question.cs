using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;

namespace Festispec.Models.Questions
{
    public abstract class Question : Entity, IAnswerable<Answer>
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

        [Required]
        public virtual Questionnaire Questionnaire { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public abstract GraphType GraphType { get; }

    }
}