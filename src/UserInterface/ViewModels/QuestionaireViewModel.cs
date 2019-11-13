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
        IQuestionnaireService questionnaireService;
        ICommand AddQuestionCommand;
        Questionnaire Questionnaire { get; set; }
        ICollection<Question> Questions
        {
            get
            {
                return Questionnaire.Questions;
            }
        }

        public QuestionaireViewModel(Questionnaire questionnaire)
        {
            questionnaireService = new QuestionnaireService(new FestispecContext());
            if (questionnaire == null)
                Questionnaire = new Questionnaire();
            
            AddQuestionCommand = new RelayCommand(AddQuestion);

        }

        public void AddQuestion()
        {

            questionnaireService.AddQuestion(Questionnaire, new StringQuestion());
        }
    } 
}