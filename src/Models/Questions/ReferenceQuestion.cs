using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace Festispec.Models.Questions
{
    public class ReferenceQuestion : Questions.Question, INotifyPropertyChanged
    {
        public ReferenceQuestion(string contents, Questionnaire questionnaire, Question question) : base(contents, questionnaire) 
        {
            Question = question;
        }
        public ReferenceQuestion() : base() { }

        private Question _question;
        public Question Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override GraphType GraphType => Question.GraphType;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}