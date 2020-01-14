using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Festispec.Models.Questions
{
    public class ReferenceQuestion : Question, INotifyPropertyChanged
    {
        private Question _question;

        public ReferenceQuestion(string contents, Questionnaire questionnaire, Question question) : base(contents,
            questionnaire)
        {
            Question = question;
        }

        public ReferenceQuestion()
        {
        }

        public Question Question
        {
            get => _question;
            set
            {
                _question = value;
                NotifyPropertyChanged();
            }
        }

        public override GraphType GraphType => Question.GraphType;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}