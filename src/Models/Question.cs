using System.Collections.Generic;

namespace Festispec.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Contents { get; set; }
        public int NumericMinimum { get; set; }
        public int NumericMaximum { get; set; }
        public bool StringIsMultiline { get; set; }
        public int StringCharacterLimit { get; set; }
        public virtual QuestionCategory Category { get; set; }
        public virtual QuestionType Type { get; set; }
        // for multiple choice answers
        public virtual ICollection<string> Options { get; set; }
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
    }
}