using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Factories;
using Festispec.Models.Questions;
using Festispec.UI.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        private ObservableCollection<Question> _questions { get; set; }
        private ObservableCollection<Question> _addedQuestions { get; set; }
        private ObservableCollection<Question> _removedQuestions { get; set; }
        public List<string> QuestionType { get => _questionFactory.QuestionTypes.ToList(); }
        public ObservableCollection<Question> Questions { get => _questions; }
        public string Selecteditem { 
            get; 
            set;
        }
        
        public QuestionnaireViewModel(IQuestionnaireService questionnaireService)
        {
            Questionnaire = new Questionnaire();
            _questions = new ObservableCollection<Question>();
            _addedQuestions = new ObservableCollection<Question>();
            _removedQuestions = new ObservableCollection<Question>();
            _questionnaireService = questionnaireService;
            _questionFactory = new QuestionFactory();
            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            DeleteQuestionCommand = new RelayCommand<Question>(DeleteQuestion);
            SaveQuestionnaireCommand = new RelayCommand(SaveQuestionnaire);
            OpenFileWindowCommand = new RelayCommand(OpenFileWindow);

        }

        public void AddQuestion()
        {
            var tempQuestion = _questionFactory.GetQuestionType(Selecteditem);
            tempQuestion.Contents = "";
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

        public void SaveQuestionnaire()
        {
            _addedQuestions.ToList().ForEach(e => _questionnaireService.AddQuestion(Questionnaire, e));
            _removedQuestions.ToList().ForEach(e => _questionnaireService.RemoveQuestion(Questionnaire, e.Id));
        }

        
        public void OpenFileWindow()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
        }

    }
}