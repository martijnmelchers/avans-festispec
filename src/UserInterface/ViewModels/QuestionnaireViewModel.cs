using Festispec.DomainServices;
using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Questions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

        public QuestionnaireViewModel(IQuestionnaireService questionnaireService, QuestionFactory questionFactory)
        {
            _questionnaireService = questionnaireService;
            Questionnaire = _questionnaireService.GetQuestionnaire(2);
            _questions = new ObservableCollection<Question>(Questionnaire.Questions);
            _addedQuestions = new ObservableCollection<Question>();
            _removedQuestions = new ObservableCollection<Question>();
            _questionFactory = questionFactory;
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

        public void DeleteQuestion(Question item)
        {
            if (_addedQuestions.Contains(item))
                _addedQuestions.Remove(item);
            else
                _removedQuestions.Add(item);
            Questions.Remove(item);
        }

        public async void SaveQuestionnaire()
        {
            var multipleChoiceQuestions = new List<MultipleChoiceQuestion>();
            multipleChoiceQuestions.AddRange(_addedQuestions.OfType<MultipleChoiceQuestion>());
            multipleChoiceQuestions.AddRange(_questions.OfType<MultipleChoiceQuestion>());

            foreach (MultipleChoiceQuestion q in multipleChoiceQuestions)
                q.ObjectsToString();

            foreach(Question q in _addedQuestions)
            {
                try
                {
                    await _questionnaireService.AddQuestion(Questionnaire.Id, q);
                    _addedQuestions.Remove(q);

                } catch  (Exception e)
                {
                    MessageBox.Show($"An error occured while adding a question. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            foreach (Question q in _removedQuestions)
            {
                try {
                    await _questionnaireService.RemoveQuestion(q.Id);
                    _removedQuestions.Remove(q);

                } catch(Exception e)
                {
                    MessageBox.Show($"An error occured while removing question with the id: {q.Id}. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
            var option = (MultipleChoiceQuestion)question;

            option.OptionCollection.Add(new StringObject());
        }

    }
}
