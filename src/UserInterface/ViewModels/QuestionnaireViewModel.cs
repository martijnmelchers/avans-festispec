using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Questions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace Festispec.UI.ViewModels
{
    internal class QuestionnaireViewModel : BaseDeleteCheckViewModel
    {
        private readonly IFestivalService _festivalService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IOfflineService _offlineService;
        private readonly QuestionFactory _questionFactory;
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IConfiguration _config;

        private bool _isOpen;
        private int _search;
        private ReferenceQuestion _selectedReferenceQuestion;
        private string _selectedItem;
        
        public QuestionnaireViewModel(IQuestionnaireService questionnaireService, QuestionFactory questionFactory,
            IFrameNavigationService navigationService, IFestivalService festivalService, IOfflineService offlineService, IConfiguration config)
        {

            _config = config;
            _questionnaireService = questionnaireService;
            _navigationService = navigationService;
            _questionFactory = questionFactory;
            _festivalService = festivalService;
            _offlineService = offlineService;

            Initialize((int) _navigationService.Parameter);

            AddedQuestions = new ObservableCollection<Question>();
            RemovedQuestions = new ObservableCollection<Question>();
            OpenDeleteCheckCommand = new RelayCommand<Question>(DeleteCommandCheck,_ => offlineService.IsOnline, true);
            AddQuestionCommand = new RelayCommand(AddQuestion, () => SelectedItem != null, true);
            // DeleteQuestionCommand = new RelayCommand<Question>(DeleteQuestion, _ => offlineService.IsOnline, true);
            DeleteCommand = new RelayCommand(DeleteQuestion,() => offlineService.IsOnline, true);
            DeleteQuestionnaireCommand = new RelayCommand(DeleteQuestionnaire, () => offlineService.IsOnline, true);
            SaveQuestionnaireCommand = new RelayCommand(SaveQuestionnaire, () => offlineService.IsOnline, true);
            OpenFileWindowCommand = new RelayCommand<Question>(OpenFileWindow, HasAnswers);
            AddOptionToQuestion = new RelayCommand<Question>(AddOption, _ => offlineService.IsOnline, true);
            SelectReferenceQuestionCommand =
                new RelayCommand<ReferenceQuestion>(SelectReferenceQuestion, _ => offlineService.IsOnline, true);
            SetReferenceQuestionCommand =
                new RelayCommand<Question>(SetReferenceQuestion, _ => offlineService.IsOnline, true);

            QuestionList = (CollectionView) CollectionViewSource.GetDefaultView(_allQuestions());
            QuestionList.Filter = Filter;
        }

        
        private Questionnaire Questionnaire { get; set; }
        public RelayCommand AddQuestionCommand { get; set; }
        public ICommand OpenDeleteCheckCommand { get; set; }
        public ICommand DeleteQuestionnaireCommand { get; set; }
        public ICommand SaveQuestionnaireCommand { get; set; }
        public ICommand OpenFileWindowCommand { get; set; }
        public ICommand SelectReferenceQuestionCommand { get; set; }
        public ICommand SetReferenceQuestionCommand { get; set; }
        public RelayCommand<Question> AddOptionToQuestion { get; set; }
        private ObservableCollection<Question> AddedQuestions { get; }
        private ObservableCollection<Question> RemovedQuestions { get; }
        public IEnumerable<string> QuestionType => _questionFactory.QuestionTypes.ToList();
        public ObservableCollection<Question> Questions { get; private set; }

        public string SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; AddQuestionCommand.RaiseCanExecuteChanged(); }
        }

        private Question SelectedQuestion { get; set; }


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

        public IEnumerable<Questionnaire> Questionnaires =>
            _festivalService.GetFestival(Questionnaire.Festival.Id).Questionnaires
                .Where(e => e.Id != Questionnaire.Id).ToList();

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

        private void Initialize(int input)
        {
            Questionnaire = _questionnaireService.GetQuestionnaire(input);
            Questions = new ObservableCollection<Question>(Questionnaire.Questions);
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
            RaisePropertyChanged(nameof(Questions));
        }

        private List<Question> _allQuestions()
        {
            return Questionnaire.Festival.Questionnaires.SelectMany(item => item.Questions).ToList();
        }

        private async void DeleteQuestionnaire()
        {
            var festivalId = Questionnaire.Festival.Id;
            await _questionnaireService.RemoveQuestionnaire(Questionnaire.Id);
            _navigationService.NavigateTo("FestivalInfo", festivalId);
        }

        private void DeleteCommandCheck(Question question)
        {
            SelectedQuestion = question;
            OpenDeletePopup();
        }
        private void AddQuestion()
        {
            var tempQuestion = _questionFactory.GetQuestionType(SelectedItem);
            AddedQuestions.Add(tempQuestion);
            Questions.Add(tempQuestion);
        }

        private void DeleteQuestion()
        {
            if (AddedQuestions.Contains(SelectedQuestion))
                AddedQuestions.Remove(SelectedQuestion);
            else
                RemovedQuestions.Add(SelectedQuestion);
            Questions.Remove(SelectedQuestion);
        }

        private async void SaveQuestionnaire()
        {
            var multipleChoiceQuestions = new List<MultipleChoiceQuestion>();
            multipleChoiceQuestions.AddRange(AddedQuestions.OfType<MultipleChoiceQuestion>());
            multipleChoiceQuestions.AddRange(Questions.OfType<MultipleChoiceQuestion>());

            foreach (var q in multipleChoiceQuestions)
                q.ObjectsToString();

            foreach (var q in AddedQuestions)
                try
                {
                    await _questionnaireService.AddQuestion(Questionnaire.Id, q);
                }
                catch (Exception)
                {
                    ValidationError = "Er is iets niet goedgegaan tijdens het toevoegen van de vraag(en)";
                    PopupIsOpen = true;

                }

            AddedQuestions.Clear();

            foreach (var q in RemovedQuestions)
                try
                {
                    await _questionnaireService.RemoveQuestion(q.Id);
                }
                catch (Exception)
                {
                    ValidationError = "Er is iets niet goedgegaan tijdens het verwijderen van de vraag(en)";
                    PopupIsOpen = true;
                }

            RemovedQuestions.Clear();
            _navigationService.NavigateTo("FestivalInfo", Questionnaire.Festival.Id);
        }

        private bool HasAnswers(Question question)
        {
            return question.Answers.Count == 0 && _offlineService.IsOnline;
        }

        private async void OpenFileWindow(Question question)
        {
            var fileDialog = new OpenFileDialog();

            var dialog = fileDialog.ShowDialog();

            // Check if a file has been selected.
            if (dialog == null || dialog != true) return;
            await using var stream = fileDialog.OpenFile();
            var url = $"{_config["Urls:WebApp"]}/Upload/UploadFile";
            var response = await UploadImage(url, stream, fileDialog.SafeFileName);
            var path = await response.Content.ReadAsStringAsync();

            if (AddedQuestions.FirstOrDefault(q => q.Equals(question)) is DrawQuestion drawQuestion) drawQuestion.PicturePath = path;
            MessageBox.Show("Het bestand is geupload.");
        }

        private static void AddOption(Question question)
        {
            var option = (MultipleChoiceQuestion) question;

            option.OptionCollection.Add(new StringObject());
        }

        private bool Filter(object item)
        {
            return Search <= 0 || ((Question) item).Questionnaire.Id == Search;
        }

        private static async Task<HttpResponseMessage> UploadImage(string url, Stream image, string fileName)
        {
            await using (var str = new MemoryStream())
            using (var client = new HttpClient())
            {

                image.CopyTo(str);
                byte[] byteArray = str.ToArray();
                var requestContent = new MultipartFormDataContent();
                //    here you can specify boundary if you need---^
                var imageContent = new ByteArrayContent(byteArray);
                imageContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("image/jpeg");

                requestContent.Add(imageContent, "image", fileName);

                var response =  await client.PostAsync(url, requestContent);

                return response;
            }
        }
    }
}
