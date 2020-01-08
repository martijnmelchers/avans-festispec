using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Questions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace Festispec.UI.ViewModels
{
    internal class QuestionnaireViewModel : ViewModelBase, IActivateable<int>
    {
        private readonly IFestivalService _festivalService;
        private readonly IOfflineService _offlineService;
        private readonly QuestionFactory _questionFactory;
        private readonly IFrameNavigationService _navigationService;
        public Questionnaire Questionnaire { get; set; }
        public ICommand AddQuestionCommand { get; set; }
        public ICommand DeleteQuestionCommand { get; set; }
        public ICommand DeleteQuestionaireCommand { get; set; }
        public ICommand SaveQuestionnaireCommand { get; set; }
        public ICommand OpenFileWindowCommand { get; set; }
        public ICommand SelectReferenceQuestionCommand { get; set; }
        public ICommand SetReferenceQuestionCommand { get; set; }
        public RelayCommand<Question> AddOptionToQuestion { get; set; }

        private bool _isOpen;

        private int _search;


        private ReferenceQuestion _selectedReferenceQuestion;

        public QuestionnaireViewModel(IQuestionnaireService questionnaireService, QuestionFactory questionFactory,
            IFrameNavigationService navigationService, IFestivalService festivalService, IOfflineService offlineService)
        {
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            _questionFactory = questionFactory;
            _festivalService = festivalService;
            _offlineService = offlineService;

            Initialize((int) _navigationService.Parameter);

            AddedQuestions = new ObservableCollection<Question>();
            _removedQuestions = new ObservableCollection<Question>();

            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            DeleteQuestionCommand = new RelayCommand<Question>(DeleteQuestion, _ => offlineService.IsOnline);
            DeleteQuestionaireCommand = new RelayCommand(DeleteQuestionaire, () => offlineService.IsOnline);
            SaveQuestionnaireCommand = new RelayCommand(SaveQuestionnaire, () => offlineService.IsOnline);
            OpenFileWindowCommand = new RelayCommand<Question>(OpenFileWindow, HasAnswers);
            AddOptionToQuestion = new RelayCommand<Question>(AddOption, _ => offlineService.IsOnline);
            ReturnCommand = new RelayCommand(NavigateToFestivalInfo);
            SelectReferenceQuestionCommand = new RelayCommand<ReferenceQuestion>(SelectReferenceQuestion, _ => offlineService.IsOnline);
            SetReferenceQuestionCommand = new RelayCommand<Question>(SetReferenceQuestion, _ => offlineService.IsOnline);

            QuestionList = (CollectionView) CollectionViewSource.GetDefaultView(_allQuestions());
            QuestionList.Filter = Filter;
        }

        public Questionnaire Questionnaire { get; set; }
        public ICommand AddQuestionCommand { get; set; }
        public ICommand DeleteQuestionCommand { get; set; }
        public ICommand DeleteQuestionaireCommand { get; set; }
        public ICommand SaveQuestionnaireCommand { get; set; }
        public ICommand OpenFileWindowCommand { get; set; }
        public ICommand SelectReferenceQuestionCommand { get; set; }
        public ICommand SetReferenceQuestionCommand { get; set; }
        public RelayCommand<Question> AddOptionToQuestion { get; set; }

        private ObservableCollection<Question> _questions { get; set; }
        public ObservableCollection<Question> AddedQuestions { get; set; }
        private ObservableCollection<Question> _removedQuestions { get; }
        public List<string> QuestionType => _questionFactory.QuestionTypes.ToList();
        public ObservableCollection<Question> Questions => _questions;
        public string Selecteditem { get; set; }


        public CollectionView QuestionList { get; }

        public int Search
        {
            get => _search;
            set
            {
                _search = value;
                QuestionList.Filter += Filter;
            }
        }

        public List<Questionnaire> Questionnaires
        {
            get
            {
                return _festivalService.GetFestival(Questionnaire.Festival.Id).Questionnaires
                    .Where(e => e.Id != Questionnaire.Id).ToList();
            }
        }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen == value) return;
                _isOpen = value;
                RaisePropertyChanged();
            }
        }

        public void Initialize(int input)
        {
            Questionnaire = _questionnaireService.GetQuestionnaire(input);
            _questions = new ObservableCollection<Question>(Questionnaire.Questions);
        }

        private void SelectReferenceQuestion(ReferenceQuestion referenceQuestion)
        {
            _selectedReferenceQuestion = referenceQuestion;
            IsOpen = true;
        }

        private void SetReferenceQuestion(Question question)
        {
            _selectedReferenceQuestion.Question = question;
            IsOpen = false;
            RaisePropertyChanged("Questions");
        }

        private List<Question> _allQuestions()
        {
            var temp = new List<Question>();
            foreach (Questionnaire item in Questionnaire.Festival.Questionnaires)
            foreach (Question item2 in item.Questions)
                temp.Add(item2);
            return temp;
        }


        private void DeleteQuestionaire()
        {
            _navigationService.NavigateTo("FestivalInfo", Questionnaire.Festival.Id);
            _questionnaireService.RemoveQuestionnaire(Questionnaire.Id);
        }

        public void AddQuestion()
        {
            Question tempQuestion = _questionFactory.GetQuestionType(Selecteditem);
            AddedQuestions.Add(tempQuestion);
            _questions.Add(tempQuestion);
        }

        public bool CanAddQuestion()
        {
            return Selecteditem != null && _offlineService.IsOnline;
        }

        public void DeleteQuestion(Question item)
        {
            if (AddedQuestions.Contains(item))
                AddedQuestions.Remove(item);
            else
                _removedQuestions.Add(item);
            Questions.Remove(item);
        }

        public void NavigateToFestivalInfo()
        {
            _navigationService.NavigateTo("FestivalInfo", Questionnaire.Festival.Id);
        }

        public async void SaveQuestionnaire()
        {
            var multipleChoiceQuestions = new List<MultipleChoiceQuestion>();
            multipleChoiceQuestions.AddRange(AddedQuestions.OfType<MultipleChoiceQuestion>());
            multipleChoiceQuestions.AddRange(_questions.OfType<MultipleChoiceQuestion>());

            foreach (MultipleChoiceQuestion q in multipleChoiceQuestions)
                q.ObjectsToString();

            foreach (Question q in AddedQuestions)
                try
                {
                    await _questionnaireService.AddQuestion(Questionnaire.Id, q);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"An error occured while adding a question. The occured error is: {e.GetType()}",
                        $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            AddedQuestions.Clear();

            foreach (Question q in _removedQuestions)
                try
                {
                    await _questionnaireService.RemoveQuestion(q.Id);
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        $"An error occured while removing question with the id: {q.Id}. The occured error is: {e.GetType()}",
                        $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            _removedQuestions.Clear();
            _navigationService.NavigateTo("FestivalInfo", Questionnaire.Festival.Id);
        }

        public bool HasAnswers(Question question)
        {
            return question.Answers.Count == 0 && _offlineService.IsOnline;
        }

        public void OpenFileWindow(Question question)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
        }

        public void AddOption(Question question)
        {
            var option = (MultipleChoiceQuestion) question;

            option.OptionCollection.Add(new StringObject());
        }

        private bool Filter(object item)
        {
            return Search <= 0 || ((Question) item).Questionnaire.Id == Search;
        }
    }
}