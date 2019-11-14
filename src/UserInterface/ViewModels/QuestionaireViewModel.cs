using Festispec.DomainServices.Interfaces;
using Festispec.Models.Questions;
using Festispec.Models;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Festispec.Models.Factories;

namespace Festispec.UI.ViewModels
{
    internal class QuestionaireViewModel : ViewModelBase
    {
        private IQuestionnaireService _questionnaireService;
        private QuestionFactory _questionFactory;
        public Questionnaire Questionnaire { get; set; }
        public ICommand AddQuestionCommand { get; set; }
        public ICommand DeleteQuestionCommand { get; set; }
        public ICommand SaveQuestionaireCommand { get; set; }
        private ObservableCollection<Question> _questions { get; set; }
        private ObservableCollection<Question> _addedQuestions { get; set; }
        private ObservableCollection<Question> _removedQuestions { get; set; }
        public List<string> QuestionType { get => _questionFactory.QuestionTypes.ToList(); }
        public ObservableCollection<Question> Questions { get => _questions; }


        public QuestionaireViewModel(IQuestionnaireService questionnaireService)
        {
            Questionnaire = new Questionnaire();
            _questions = new ObservableCollection<Question>();
            _addedQuestions = new ObservableCollection<Question>();
            _removedQuestions = new ObservableCollection<Question>();
            _questionnaireService = questionnaireService;
            _questionFactory = new QuestionFactory();
            AddQuestionCommand = new RelayCommand(AddQuestion);
            DeleteQuestionCommand = new RelayCommand<Question>(DeleteQuestion);
            SaveQuestionaireCommand = new RelayCommand(SaveQuestionnaire);
        }

        public void AddQuestion()
        {
            _addedQuestions.Add(new StringQuestion());
            _questions.Add(new StringQuestion());
        }

        public void DeleteQuestion(object item)
        {
            _removedQuestions.Add(item as Question);
            Questions.Remove(item as Question);
        }

        public void SaveQuestionnaire()
        {
            _addedQuestions.ToList().ForEach(e => _questionnaireService.AddQuestion(Questionnaire, e));
            _removedQuestions.ToList().ForEach(e => _questionnaireService.RemoveQuestion(Questionnaire, e.Id));
        }
    }
}