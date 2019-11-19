using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Festispec.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        //public virtual ICollection<string> Options { get; set; }
        public virtual string Answer1 { get; set; }
        public virtual string Answer2 { get; set; }
        public virtual string Answer3 { get; set; }
        public virtual string Answer4 { get; set; }
        public override GraphType GraphType => GraphType.Pie;

        public MultipleChoiceQuestion()
        {
            //Options = new ObservableCollection<string>();
            
        }
    }
}