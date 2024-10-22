using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Festispec.Models.Attributes;

namespace Festispec.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        private static readonly string STRING_SEPERATOR = "~";
        private string _options;

        public MultipleChoiceQuestion(string contents, Questionnaire questionnaire) : base(contents, questionnaire) 
        {
        }

        public MultipleChoiceQuestion()
        {
            OptionCollection = new ObservableCollection<StringObject>();
        }

        public override GraphType GraphType => GraphType.Pie;

        // This property contains the options comma seperated
        public string Options
        {
            get => _options;
            set { _options = value; StringToObjects(); }
        }

        [NotMapped]
        [Required]
        [ListElements(1)]
        public ObservableCollection<StringObject> OptionCollection { get; set; }

        public void ObjectsToString()
        {
            Options = string.Join(STRING_SEPERATOR, OptionCollection);
        }

        public void StringToObjects()
        {
            OptionCollection =
                new ObservableCollection<StringObject>(Options.Split(STRING_SEPERATOR)
                    .Select(str => new StringObject(str)));
        }
    }
}