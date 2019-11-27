using Festispec.Models.Answers;
using Festispec.Models.Attributes;
using Festispec.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Festispec.Models.Questions
{
    public class MultipleChoiceQuestion : Question, IAnswerable<MultipleChoiceAnswer>
    {
        private static readonly string STRING_SEPERATOR = "~";
        public MultipleChoiceQuestion(string contents, Questionnaire questionnaire) : base(contents, questionnaire) 
        {
        }
        public MultipleChoiceQuestion() : base() {
            OptionCollection = new ObservableCollection<StringObject>();
        }

        public override GraphType GraphType => GraphType.Pie;
        public new virtual ICollection<MultipleChoiceAnswer> Answers { get; set; }
        
        // This property contains the options comma seperated
        public string Options { get; set; }

        [NotMapped, Required, ListElements(1)]
        public ObservableCollection<StringObject> OptionCollection { get; set; }

        public void ObjectsToString()
        {
            Options = string.Join(STRING_SEPERATOR, OptionCollection);
        }

        public void StringToObjects()
        {
            OptionCollection = new ObservableCollection<StringObject>(Options.Split(STRING_SEPERATOR).Select(str => new StringObject(str)));
        }


    }
}