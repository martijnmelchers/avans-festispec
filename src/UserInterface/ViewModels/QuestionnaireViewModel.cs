using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Factories;
using Festispec.Models.Questions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    class QuestionnaireViewModel : ViewModelBase
    {
        private IQuestionnaireService _questionnaireService;
        private QuestionFactory _questionFactory;
        public Questionnaire Questionnaire { get; set; }
        public ICommand AddQuestionCommand { get; set; }
        public ICommand DeleteQuestionCommand { get; set; }
        public ICommand SaveQuestionnaireCommand { get; set; }
        public ICommand OpenFileWindowCommand { get; set; }
        public RelayCommand<Question> AddOptionToQuestion { get; set; }

        private ObservableCollection<Question> _questions { get; set; }
        private ObservableCollection<Question> _addedQuestions { get; set; }
        private ObservableCollection<Question> _removedQuestions { get; set; }
        public List<string> QuestionType { get => _questionFactory.QuestionTypes.ToList(); }
        public ObservableCollection<Question> Questions { get => _questions; }
        public string Selecteditem { get; set; }

        public QuestionnaireViewModel(IQuestionnaireService questionnaireService)
        {
            _questionnaireService = questionnaireService;
            Questionnaire = _questionnaireService.GetQuestionnaire(2);
            _questions = new ObservableCollection<Question>(Questionnaire.Questions);
            _addedQuestions = new ObservableCollection<Question>();
            _removedQuestions = new ObservableCollection<Question>();
            _questionFactory = new QuestionFactory();
            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            DeleteQuestionCommand = new RelayCommand<Question>(DeleteQuestion);
            SaveQuestionnaireCommand = new RelayCommand(SaveQuestionnaire);
            OpenFileWindowCommand = new RelayCommand<Question>(OpenFileWindow,HasAnswers);
            AddOptionToQuestion = new RelayCommand<Question>(AddOption);
        }

        public void AddQuestion()
        {
            var tempQuestion = _questionFactory.GetQuestionType(Selecteditem);
            _addedQuestions.Add(tempQuestion);
            _questions.Add(tempQuestion);
        }

        public bool CanAddQuestion()
        {
            return Selecteditem != null;
        }

        public void DeleteQuestion(object item)
        {
            _removedQuestions.Add(item as Question);
            Questions.Remove(item as Question);
        }

        public async void SaveQuestionnaire()
        {
            var multipleChoiceQuestions = new List<MultipleChoiceQuestion>();
            multipleChoiceQuestions.AddRange(_addedQuestions.OfType<MultipleChoiceQuestion>());
            multipleChoiceQuestions.AddRange(_questions.OfType<MultipleChoiceQuestion>());

            foreach(MultipleChoiceQuestion q in multipleChoiceQuestions)
                q.Options = string.Join(",", q.OptionCollection);

            _addedQuestions.ToList().ForEach(async e => await _questionnaireService.AddQuestion(Questionnaire, e));
            _addedQuestions.Clear();

            //Als je dit in de loop zet is geeft bool success aan of hij gelukt is 
            //(bool success, Question question) = await _questionnaireService.AddQuestion(Questionnaire, e);

            foreach (Question q in _removedQuestions)
            {
                if (await _questionnaireService.RemoveQuestion(q.Id))
                    _removedQuestions.Remove(q);
            }
        }

        public bool HasAnswers(Question question)
        {
            return question.Answers.Count == 0;
        }

        public void OpenFileWindow(Question question)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
        }

        public void AddOption(Question question)
        {
            var x = (MultipleChoiceQuestion)question;

            x.OptionCollection.Add(new StringObject());
        }

    }
}