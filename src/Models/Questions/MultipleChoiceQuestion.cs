using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class MultipleChoiceQuestion : Question, IAnswerable<MultipleChoiceAnswer>
    {
        public MultipleChoiceQuestion(string contents, Questionnaire questionnaire, string answer1) : base(contents, questionnaire) 
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
        public new virtual ICollection<MultipleChoiceAnswer> Answers { get; set; }
        public ICollection<string> Options { get; internal set; }
    }
}