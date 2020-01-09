using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Questions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Services;

namespace Festispec.UI.ViewModels
{
    class QuestionnaireViewModel : BaseValidationViewModel, IActivateable<int>
    {
        private readonly IQuestionnaireService _questionnaireService;
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
        public ICommand ReturnCommand { get; set; }
        public ICommand SelectReferenceQuestionCommand { get; set; }
        public ICommand SetReferenceQuestionCommand { get; set; }
        public RelayCommand<Question> AddOptionToQuestion { get; set; }

        private ObservableCollection<Question> _questions { get; set; }
        public ObservableCollection<Question> AddedQuestions { get; set; }
        private ObservableCollection<Question> _removedQuestions { get; set; }
        public List<string> QuestionType { get => _questionFactory.QuestionTypes.ToList(); }
        public ObservableCollection<Question> Questions { get => _questions; }
        public string Selecteditem { get; set; }

        public QuestionnaireViewModel(IQuestionnaireService questionnaireService, QuestionFactory questionFactory, IFrameNavigationService navigationService, IFestivalService festivalService, IOfflineService offlineService)
        {
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            _questionFactory = questionFactory;
            _festivalService = festivalService;
            _offlineService = offlineService;

            Initialize((int)_navigationService.Parameter);

            AddedQuestions = new ObservableCollection<Question>();
            _removedQuestions = new ObservableCollection<Question>();


            AddQuestionCommand = new RelayCommand(AddQuestion);
            DeleteQuestionCommand = new RelayCommand<Question>(DeleteQuestion, _ => offlineService.IsOnline);
            DeleteQuestionaireCommand = new RelayCommand(DeleteQuestionaire, () => offlineService.IsOnline);
            SaveQuestionnaireCommand = new RelayCommand(SaveQuestionnaire, () => offlineService.IsOnline);
            OpenFileWindowCommand = new RelayCommand<Question>(OpenFileWindow, HasAnswers);
            AddOptionToQuestion = new RelayCommand<Question>(AddOption, _ => offlineService.IsOnline);
            ReturnCommand = new RelayCommand(NavigateToFestivalInfo);
            SelectReferenceQuestionCommand = new RelayCommand<ReferenceQuestion>(SelectReferenceQuestion, _ => offlineService.IsOnline);
            SetReferenceQuestionCommand = new RelayCommand<Question>(SetReferenceQuestion, _ => offlineService.IsOnline);

            QuestionList = (CollectionView)CollectionViewSource.GetDefaultView(_allQuestions());
            QuestionList.Filter = Filter;
        }


        private ReferenceQuestion _selectedReferenceQuestion;
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
            List<Question> temp = new List<Question>();
            foreach (var item in Questionnaire.Festival.Questionnaires)
            {
                foreach (var item2 in item.Questions)
                {
                    temp.Add(item2);
                }
            }
            return temp;

        }


        private void DeleteQuestionaire()
        {
            _navigationService.NavigateTo("FestivalInfo", Questionnaire.Festival.Id);
            _questionnaireService.RemoveQuestionnaire(Questionnaire.Id);
        }

        public void AddQuestion()
        {
            if (Selecteditem == null)
            {
                ValidationError = $"Selecteer eerst een vraagtype";
                PopupIsOpen = true;
            }
            else
            {
                var tempQuestion = _questionFactory.GetQuestionType(Selecteditem);
                AddedQuestions.Add(tempQuestion);
                _questions.Add(tempQuestion);
            }
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
            {
                try
                {
                    await _questionnaireService.AddQuestion(Questionnaire.Id, q);
                }
                catch (Exception e)
                {
                    ValidationError = $"Vraag kan niet worden toegevoegd";
                    PopupIsOpen = true;
                }
            }
            AddedQuestions.Clear();

            foreach (Question q in _removedQuestions)
            {
                try
                {
                    await _questionnaireService.RemoveQuestion(q.Id);

                }
                catch (Exception e)
                {
                    ValidationError = $"Vraag kan niet worden verwijderd";
                    PopupIsOpen = true;
                }
            }
            _removedQuestions.Clear();
            if (!PopupIsOpen)
                _navigationService.NavigateTo("FestivalInfo", Questionnaire.Festival.Id);
        }

        public bool HasAnswers(Question question)
        {
            return question.Answers.Count == 0 && _offlineService.IsOnline;
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

        public void Initialize(int input)
        {
            Questionnaire = _questionnaireService.GetQuestionnaire(input);
            _questions = new ObservableCollection<Question>(Questionnaire.Questions);
            _questionnaireService.Sync();
        }


        public CollectionView QuestionList { get; }

        private bool Filter(object item) => Search <= 0 || ((Question)item).Questionnaire.Id == Search;

        private int _search;

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
                return _festivalService.GetFestival(Questionnaire.Festival.Id).Questionnaires.Where(e => e.Id != Questionnaire.Id).ToList();
            }
        }

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen == value) return;
                _isOpen = value;
                RaisePropertyChanged("IsOpen");
            }
        }
    }
}
