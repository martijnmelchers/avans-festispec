using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        public MultipleChoiceQuestion(string contents, QuestionCategory category, Questionnaire questionnaire, string answer1) : base(contents, category, questionnaire) 
        {
            Answer1 = answer1;
        }
        public MultipleChoiceQuestion() : base() { }

        [Required]
        public virtual string Answer1 { get; set; }
        public virtual string Answer2 { get; set; }
        public virtual string Answer3 { get; set; }
        public virtual string Answer4 { get; set; }
        public override GraphType GraphType => GraphType.Pie;
    }
}