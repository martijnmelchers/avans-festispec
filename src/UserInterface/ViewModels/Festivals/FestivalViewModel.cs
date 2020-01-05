using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    public class FestivalViewModel : ViewModelBase, IAsyncActivateable<int>
    {
        private readonly IFestivalService _festivalService;
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IFrameNavigationService _navigationService;

        private Festival _festival;
        public Festival Festival
        {
            get { return _festival; }
            set { _festival = value; RaisePropertyChanged(nameof(Festival)); }
        }
        public string FestivalLocation { get => Festival?.Address?.ToString() ?? "Laden..."; }
        public string FestivalData { get; set; }
        public string FestivalTimes { get; set; }
        public string QuestionnaireName { get; set; }
        private int _deletetingQuestionnareId { get; set; }

        public ICommand EditFestivalCommand { get; set; }
        public ICommand RemoveFestivalCommand { get; set; }
        public ICommand CreateQuestionnaireCommand { get; set; }
        public ICommand ConfirmDeleteQuestionnaireCommand { get; set; }
        public ICommand GenerateReportCommand { get; set; }
        public RelayCommand<int> OpenQuestionnaireCommand { get; set; }
        public RelayCommand<int> DeleteQuestionnaireCommand { get; set; }

        public FestivalViewModel(IFrameNavigationService navigationService, IFestivalService festivalService, IQuestionnaireService questionnaireService)
        {
            _festivalService = festivalService;
            _navigationService = navigationService;
            _questionnaireService = questionnaireService;

            RemoveFestivalCommand = new RelayCommand(RemoveFestival);
            EditFestivalCommand = new RelayCommand(EditFestival);
            OpenQuestionnaireCommand = new RelayCommand<int>(OpenQuestionnaire);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire);
            ConfirmDeleteQuestionnaireCommand = new RelayCommand(DeleteQuestionnaire);
            DeleteQuestionnaireCommand = new RelayCommand<int>(PrepareQuestionnaireDelete);
            GenerateReportCommand = new RelayCommand(GenerateReport);



            Task.Run(async () => await Initialize((int)_navigationService.Parameter));
        }

        public async Task Initialize(int id)
        {
            Festival = await _festivalService.GetFestivalAsync(id);

            FestivalData = Festival.OpeningHours.StartDate.ToString("dd/MM/yyyy") + " - " + Festival.OpeningHours.EndDate.ToString("dd/MM/yyyy");
            FestivalTimes = Festival.OpeningHours.StartTime.ToString(@"hh\:mm") + " - " + Festival.OpeningHours.EndTime.ToString(@"hh\:mm");

            RaisePropertyChanged(nameof(FestivalData));
            RaisePropertyChanged(nameof(FestivalLocation));
            RaisePropertyChanged(nameof(FestivalTimes));
        }

        public void EditFestival()
        {
            _navigationService.NavigateTo("UpdateFestival", Festival.Id);
        }

        public async void RemoveFestival()
        {
            try
            {
                await _festivalService.RemoveFestival(Festival.Id);
                _navigationService.NavigateTo("FestivalList");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while removing festival with the id: {Festival.Id}. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenQuestionnaire(int id)
        {
            _navigationService.NavigateTo("Questionnaire", id);
        }

        public async void CreateQuestionnaire()
        {
            try
            {
                var questionnaire = await _questionnaireService.CreateQuestionnaire(QuestionnaireName, Festival);
                _festivalService.Sync();
                OpenQuestionnaire(questionnaire.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Er is een fout opgetreden tijdens het aanmaken van de vragenlijst. Probeer het opnieuw.", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteQuestionnaire()
        {
            if (_deletetingQuestionnareId <= 0)
                return;

            try
            {
                await _questionnaireService.RemoveQuestionnaire(_deletetingQuestionnareId);
                _festivalService.Sync();
               
            } catch(QuestionHasAnswersException e)
            {
                MessageBox.Show($"Deze vragenlijst kan niet worden verwijderd omdat er al vragen zijn beantwoord.", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrepareQuestionnaireDelete(int id)
        {
            _deletetingQuestionnareId = id;
        }


        private void GenerateReport()
        {
            _navigationService.NavigateTo("GenerateReport", Festival.Id);
        }
    }
}
