using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public abstract class Question : Entity
    {
        public Question(string contents, QuestionCategory category, Questionnaire questionnaire)
        {
            Contents = contents;
            Category = category;
            Questionnaire = questionnaire;
        }
        public int Id { get; set; }

        [Required, MinLength(5), MaxLength(250)]
        public string Contents { get; set; }
        
        [Required]
        public virtual QuestionCategory Category { get; set; }

        [Required]
        public virtual Questionnaire Questionnaire { get; set; }

        public abstract GraphType GraphType { get; }

    }
}