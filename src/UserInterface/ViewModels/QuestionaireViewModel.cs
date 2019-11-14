using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models.EntityMapping;
using Festispec.Models;
using Festispec.Models.Questions;
using System.Collections.ObjectModel;

namespace Festispec.UI.ViewModels
{
    class QuestionaireViewModel : ViewModelBase
    {
        IQuestionnaireService _questionnaireService;
        public Questionnaire Questionnaire { get; set; }
        public ICommand AddQuestionCommand { get; set; }
        public ICommand DeleteQuestionCommand { get; set; }
        public ICommand SaveQuestionaireCommand { get; set; }
        private ObservableCollection<Question> _questions { get; set; }
        public List<string> QuestionType { get; set; }

        public ObservableCollection<Question> Questions
        {
            get
            {
                return _questions;
            }
        }

        public QuestionaireViewModel(IQuestionnaireService questionnaireService)
        {
            Questionnaire = new Questionnaire();
            _questions = new ObservableCollection<Question>();
            _questionnaireService = questionnaireService;
            
            AddQuestionCommand = new RelayCommand(AddQuestion);
            DeleteQuestionCommand = new RelayCommand<Question>(DeleteQuestion);
            SaveQuestionaireCommand = new RelayCommand(SaveQuestionnaire);

        }

        public void AddQuestion()
        {
            Questions.Add(new StringQuestion());
        }

        public void DeleteQuestion(object item)
        {
            Questions.Remove(item as Question);
        }

        public void SaveQuestionnaire()
        {
            Questionnaire.Questions = Questions;
        }
    } 
}