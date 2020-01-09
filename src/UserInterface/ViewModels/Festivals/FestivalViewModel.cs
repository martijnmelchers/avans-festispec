using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Festispec.UI.ViewModels.Festivals
{
    public class FestivalViewModel : BaseDeleteCheckViewModel
    {
        private readonly IFestivalService _festivalService;
        private readonly IInspectionService _inspectionService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IQuestionnaireService _questionnaireService;

        private Festival _festival;

        private bool _createQuestionnairePopupIsOpen { get; set; } = false;
        private bool _copyQuestionnairePopupIsOpen { get; set; } = false;

        public FestivalViewModel(IFrameNavigationService navigationService, IFestivalService festivalService,
            IQuestionnaireService questionnaireService, IInspectionService inspectionService, IOfflineService offlineService)
        {
            _festivalService = festivalService;
            _navigationService = navigationService;
            _questionnaireService = questionnaireService;
            _inspectionService = inspectionService;

            RemoveFestivalCommand = new RelayCommand(OpenDeletePopup, () => offlineService.IsOnline, true);
            DeleteCommand = new RelayCommand(RemoveFestival, () => offlineService.IsOnline, true);
            EditFestivalCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateFestival", Festival.Id), () => offlineService.IsOnline, true);
            OpenQuestionnaireCommand = new RelayCommand<int>(OpenQuestionnaire);
            CreateQuestionnaireCommand = new RelayCommand(CreateQuestionnaire, () => offlineService.IsOnline, true);
            ConfirmDeleteQuestionnaireCommand = new RelayCommand(DeleteQuestionnaire, () => offlineService.IsOnline, true);
            DeleteQuestionnaireCommand = new RelayCommand<int>(id => _deletetingQuestionnareId = id, _ => offlineService.IsOnline, true);
            NewQuestionnaireCommand = new RelayCommand(NewQuestionnaire);
            OpenCopyQuestionnaireCommand = new RelayCommand<int>(openCopyQuestionnaire);
            CopyQuestionnaireCommand = new RelayCommand(CopyQuestionnaire);
            GenerateReportCommand = new RelayCommand(GenerateReport);
            DeletePlannedInspectionsCommand = new RelayCommand<List<PlannedInspection>>(DeletePlannedInspection, _ => offlineService.IsOnline, true);
            EditPlannedInspectionCommand = new RelayCommand<List<PlannedInspection>>(plannedInspections =>
            _navigationService.NavigateTo("Inspection", new {PlannedInspectionId = plannedInspections[0].Id, FestivalId = -1}), _ => offlineService.IsOnline, true);
            CreatePlannedInspectionCommand = new RelayCommand(() => _navigationService.NavigateTo("Inspection", new {PlannedInspectionId = -1, FestivalId = Festival.Id}), () => offlineService.IsOnline, true);

            CanEdit = offlineService.IsOnline;

            Initialize((int) _navigationService.Parameter);
        }

        public Festival Festival
        {
            get => _festival;
            set
            {
                _festival = value;
                RaisePropertyChanged(nameof(Festival));
            }
        }
        public bool CreateQuestionnairePopupIsOpen
        {
            get => _createQuestionnairePopupIsOpen;
            set
            {
                _createQuestionnairePopupIsOpen = value;
                RaisePropertyChanged(nameof(CreateQuestionnairePopupIsOpen));
            }
        } 
        public bool CopyQuestionnairePopupIsOpen
        {
            get => _copyQuestionnairePopupIsOpen;
            set
            {
                _copyQuestionnairePopupIsOpen = value;
                RaisePropertyChanged(nameof(CopyQuestionnairePopupIsOpen));
            }
        } 

        public bool CanEdit { get; set; }

        public string FestivalLocation => Festival?.Address?.ToString() ?? "Laden...";
        public string FestivalData { get; set; }
        public string FestivalTimes { get; set; }
        public string QuestionnaireName { get; set; }
        private int _copyQuestionnaireId { get; set; }
        private int _deletetingQuestionnareId { get; set; }

        public ICommand EditFestivalCommand { get; set; }
        public ICommand RemoveFestivalCommand { get; set; }
        public ICommand CreateQuestionnaireCommand { get; set; }
        public ICommand ConfirmDeleteQuestionnaireCommand { get; set; }
        public ICommand NewQuestionnaireCommand { get; set; }
        public ICommand CopyQuestionnaireCommand { get; set; }
        public ICommand OpenCopyQuestionnaireCommand { get; set; }
        public ICommand GenerateReportCommand { get; set; }
        public RelayCommand<int> OpenQuestionnaireCommand { get; set; }
        public RelayCommand<int> DeleteQuestionnaireCommand { get; set; }

        public ICommand DeletePlannedInspectionsCommand { get; set; }
        public ICommand EditPlannedInspectionCommand { get; set; }
        public ICommand CreatePlannedInspectionCommand { get; set; }

        public void Initialize(int id)
        {
            Festival = _festivalService.GetFestival(id);

            FestivalData = Festival.OpeningHours.StartDate.ToString("dd/MM/yyyy") + " - " +
                           Festival.OpeningHours.EndDate.ToString("dd/MM/yyyy");
            FestivalTimes = Festival.OpeningHours.StartTime.ToString(@"hh\:mm") + " - " +
                            Festival.OpeningHours.EndTime.ToString(@"hh\:mm");
            RaisePropertyChanged(nameof(FestivalData));
            RaisePropertyChanged(nameof(FestivalLocation));
            RaisePropertyChanged(nameof(FestivalTimes));
            RaisePropertyChanged(nameof(PlannedInspections));
        }

        public async void RemoveFestival()
        {
            try
            {
                await _festivalService.RemoveFestival(Festival.Id);
                _navigationService.NavigateTo("FestivalList");
            }
            catch (FestivalHasQuestionnairesException)
            {
                OpenValidationPopup("Dit festival kan niet worden verwijderd omdat er al vragenlijsten zijn aangemaakt.");
            }
        }

        public void OpenQuestionnaire(int id) => _navigationService.NavigateTo("Questionnaire", id);


        public void NewQuestionnaire() => CreateQuestionnairePopupIsOpen = true;

        public async void CreateQuestionnaire()
        {
            CreateQuestionnairePopupIsOpen = false;
            try
            {
                var questionnaire = await _questionnaireService.CreateQuestionnaire(QuestionnaireName, Festival.Id);
                _festivalService.Sync();
                OpenQuestionnaire(questionnaire.Id);
            }
            catch (Exception e)
            {
                OpenValidationPopup("Er is een fout opgetreden tijdens het aanmaken van de vragenlijst. Probeer het opnieuw.");
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
            }
            catch(QuestionHasAnswersException)
            {
                OpenValidationPopup("Deze vragenlijst kan niet worden verwijderd omdat er al vragen zijn beantwoord.");
            }
        }
        public void openCopyQuestionnaire(int copyQuestionnaireId)
        {
            CopyQuestionnairePopupIsOpen = true;
            _copyQuestionnaireId = copyQuestionnaireId;
        }

        private async void CopyQuestionnaire()
        {
            try
            {
                Questionnaire newQuestionnaire = await _questionnaireService.CopyQuestionnaire(_copyQuestionnaireId, QuestionnaireName);
                CopyQuestionnairePopupIsOpen = false;
                _navigationService.NavigateTo("Questionnaire", newQuestionnaire.Id);
            }
            catch (Exception)
            {
                OpenValidationPopup("De vragenlijst niet copieren.");
            }
        }

        private void GenerateReport()
        {
            _navigationService.NavigateTo("GenerateReport", Festival.Id);
        }

        #region PlannedInspections

        public IEnumerable<IEnumerable<PlannedInspection>> PlannedInspections => 
            Festival != null 
                ? _inspectionService.GetPlannedInspectionsGrouped(Festival)
                : new List<List<PlannedInspection>>();

        public async void DeletePlannedInspection(List<PlannedInspection> plannedInspections)
        {
            foreach (PlannedInspection plannedInspection in plannedInspections)
                try
                {
                    await _inspectionService.RemoveInspection(plannedInspection.Id, "Niet meer nodig");
                }
                catch (QuestionHasAnswersException)
                {
                    OpenValidationPopup("De inspectie kan niet worden verwijderd omdat er een vraag met antwoorden in zit.");
                }
                catch (InvalidDataException)
                {
                    OpenValidationPopup("De inspectie kan niet worden verwijderd omdat de ingevulde gegevens niet voldoen.");
                }

            RaisePropertyChanged(nameof(PlannedInspections));
        }

        #endregion PlannedInspections
    }
}