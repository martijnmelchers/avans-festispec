using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Festispec.Models.Questions
{
    public class ReferenceQuestion : Questions.Question
    {
        public ReferenceQuestion(string contents, QuestionCategory category, Questionnaire questionnaire, Question question) : base(contents, category, questionnaire) 
        {
            Question = question;
        }
        public ReferenceQuestion() : base() { }
<<<<<<< HEAD

        [Required]
        public Question Question { get; set; }
=======
        public Question Question { get; set; }
>>>>>>> 1d51df4b56e0aacc44fc5c1e8436e99fe115f0cd

        public override GraphType GraphType => Question.GraphType;
    }
}