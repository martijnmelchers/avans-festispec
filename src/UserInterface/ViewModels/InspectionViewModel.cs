﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    internal class InspectionViewModel : BaseDeleteCheckViewModel
    {
        private readonly IGoogleMapsService _googleService;
        private readonly IInspectionService _inspectionService;
        private readonly IFrameNavigationService _navigationService;

        private DateTime _endTime;

        private DateTime _originalStartTime;

        private string _search;

        private DateTime _selectedDate;

        private DateTime _startTime;
        private Festival _festival;
        private Questionnaire _selectedQuestionnaire;

        public InspectionViewModel(
            IInspectionService inspectionService,
            IFrameNavigationService navigationService,
            IGoogleMapsService googleService,
            IOfflineService offlineService
        )
        {
            _inspectionService = inspectionService;
            _navigationService = navigationService;
            _googleService = googleService;

            CheckBoxCommand = new RelayCommand<AdvancedEmployee>(CheckBox);
            SaveCommand = new RelayCommand(Save);
            ReturnCommand = new RelayCommand(() => _navigationService.NavigateTo("FestivalInfo", Festival.Id));
            OpenDeleteCheckPopup = new RelayCommand(OpenDeletePopup);
            DeleteCommand = new RelayCommand(DeletePlannedInspection, () => offlineService.IsOnline, true);

            PlannedInspections = new List<PlannedInspection>();
            Questionnaires = new List<Questionnaire>();
            OriginalPlannedInspectionIds = new List<int>();

            Task.Run(() => Initialize(_navigationService.Parameter)).Wait();
        }

        public ICollection<PlannedInspection> PlannedInspections { get; private set; }
        public string Instructions { get; set; }
        private ICollection<int> OriginalPlannedInspectionIds { get; set; }

        public Festival Festival
        {
            get => _festival;
            set { _festival = value; RaisePropertyChanged(); }
        }

        public ICommand CheckBoxCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        public ICommand OpenDeleteCheckPopup { get; set; }

        public ICollection<Questionnaire> Questionnaires { get; private set; }

        public Questionnaire SelectedQuestionnaire
        {
            get => _selectedQuestionnaire;
            set { _selectedQuestionnaire = value; RaisePropertyChanged(); }
        }

        public ICollectionView Employees { get; set; }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                Employees.Filter += Filter;
            }
        }

        public List<DateTime> GetDateOptions => Festival == null
            ? new List<DateTime>()
            : EachDay(Festival.OpeningHours.StartDate, Festival.OpeningHours.EndDate).ToList();

        public DateTime SelectedDate
        {
            get =>
                GetDateOptions.FirstOrDefault(e =>
                    e.Year == _startTime.Year && e.Month == _startTime.Month && e.Day == _startTime.Day);
            set
            {
                _selectedDate = value;
                //Set start time
                DateTime.TryParse(
                    $"{_selectedDate.Day}/{_selectedDate.Month}/{_selectedDate.Year} {_startTime.Hour}:{_startTime.Minute}",
                    out DateTime outVar);
                _startTime = outVar;
                //Set end time
                DateTime.TryParse(
                    $"{_selectedDate.Day}/{_selectedDate.Month}/{_selectedDate.Year} {_endTime.Hour}:{_endTime.Minute}",
                    out outVar);
                _endTime = outVar;
                Employees.Filter += Filter;
            }
        }

        public string StartTime
        {
            get => $"{_startTime.Hour.ToString().PadLeft(2, '0')}:{_startTime.Minute.ToString().PadLeft(2, '0')}";
            set
            {
                string dateString =
                    $"{_startTime.Day}/{_startTime.Month}/{_startTime.Year} {int.Parse(value.Substring(0, 2))}:{int.Parse(value.Substring(3, 2))}";
                DateTime.TryParse(dateString, out DateTime outVar);
                _startTime = outVar;
                Employees.Filter += Filter;
            }
        }

        public string EndTime
        {
            get => $"{_endTime.Hour.ToString().PadLeft(2, '0')}:{_endTime.Minute.ToString().PadLeft(2, '0')}";
            set
            {
                string dateString =
                    $"{_endTime.Day}/{_endTime.Month}/{_endTime.Year} {int.Parse(value.Substring(0, 2))}:{int.Parse(value.Substring(3, 2))}";
                DateTime.TryParse(dateString, out DateTime outVar);
                _endTime = outVar;
                Employees.Filter += Filter;
            }
        }

        private bool Filter(object item)
        {
            if (!(item is AdvancedEmployee advancedEmployee))
                return false;

            Employee employee = advancedEmployee.Employee;
            if (!EmployeeHasNoPlannedInspection(employee) || !EmployeeIsAvailable(employee)) return false;

            if (string.IsNullOrEmpty(Search))
                return true;
            return employee.Name.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool EmployeeIsAvailable(Employee employee)
        {
            return employee.PlannedEvents.OfType<Availability>().All(item =>
                _endTime != null && 
                (
                    _startTime.Ticks < item.StartTime.Ticks && _endTime.Ticks < item.StartTime.Ticks
                    || _startTime.Ticks > ((DateTime)item.EndTime).Ticks && _endTime.Ticks > ((DateTime)item.EndTime).Ticks)
                );
        }

        private bool EmployeeHasNoPlannedInspection(Employee employee)
        {
            foreach (PlannedInspection item in employee.PlannedEvents.ToList().OfType<PlannedInspection>())
            {
                // check if new or edit
                if (_originalStartTime == _startTime && _originalStartTime.Year > 100)
                    return true;

                if ((_startTime.Ticks >= item.StartTime.Ticks || _endTime.Ticks >= item.StartTime.Ticks) &&
                    (_startTime.Ticks <= ((DateTime)item.EndTime).Ticks || _endTime.Ticks <= ((DateTime)item.EndTime).Ticks))
                    return false;
            }

            return true;
        }

        private async Task Initialize(dynamic parameter)
        {
            if (parameter.PlannedInspectionId > 0)
            {
                PlannedInspection temp = await _inspectionService.GetPlannedInspection(parameter.PlannedInspectionId);
                Festival = temp.Festival;
                Questionnaires = temp.Festival.Questionnaires;
                _startTime = temp.StartTime;
                _endTime = (DateTime)temp.EndTime;
                SelectedQuestionnaire = temp.Questionnaire;
                _selectedDate = temp.StartTime;
                Instructions = temp.Instructions;

                PlannedInspections = await _inspectionService.GetPlannedInspections(temp.Festival.Id, temp.StartTime);
                OriginalPlannedInspectionIds = PlannedInspections.Select(pi => pi.Id).ToList();
            }
            else if (parameter.FestivalId > 0)
            {
                Festival = await _inspectionService.GetFestivalAsync(parameter.FestivalId);
                Questionnaires = Festival.Questionnaires;
            }

            if (Festival == null) throw new Exception();
            
            var advancedEmployees = new List<AdvancedEmployee>();
            foreach (Employee employee in _inspectionService.GetAllInspectors())
            {
                double distance = await _googleService.CalculateDistance(Festival.Address, employee.Address);

                advancedEmployees.Add(new AdvancedEmployee
                {
                    Employee = employee,
                    Distance = $"{distance} km",
                    DoubleDistance = distance
                });
            }

            Employees =
                (CollectionView) CollectionViewSource.GetDefaultView(advancedEmployees.OrderBy(e => e.DoubleDistance));

            _originalStartTime = _startTime;
            RaisePropertyChanged(nameof(Festival));
            RaisePropertyChanged(nameof(Questionnaire));
            RaisePropertyChanged(nameof(GetDateOptions));
            RaisePropertyChanged(nameof(StartTime));
            RaisePropertyChanged(nameof(EndTime));
            RaisePropertyChanged(nameof(SelectedDate));
            RaisePropertyChanged(nameof(CheckBox));
            RaisePropertyChanged(nameof(Employees));

            _inspectionService.Sync();
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (DateTime day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private void CheckBox(AdvancedEmployee advancedEmployee)
        {
            PlannedInspection existing =
                PlannedInspections.FirstOrDefault(pi => pi.Employee.Id == advancedEmployee.Employee.Id);

            if (existing == null)
            {
                PlannedInspections.Add(new PlannedInspection
                {
                    StartTime = _startTime,
                    EndTime = _endTime,
                    EventTitle = $"Ingeplande inspectie voor {Festival.FestivalName}",
                    Employee = advancedEmployee.Employee,
                    Questionnaire = SelectedQuestionnaire,
                    Festival = Festival
                });
            }
            else
                PlannedInspections.Remove(existing);
        }
        
        private async void DeletePlannedInspection()
        {
            ICollection<int> keptPlannedInspectionIds = new List<int>(OriginalPlannedInspectionIds);
            foreach (int plannedInspectionId in OriginalPlannedInspectionIds)
            {
                PlannedInspection plannedInspection = await _inspectionService.GetPlannedInspection(plannedInspectionId);
                try
                {
                    await _inspectionService.RemoveInspection(plannedInspection.Id, "Niet meer nodig");
                    keptPlannedInspectionIds.Remove(plannedInspectionId);
                    
                    if (PlannedInspections.Any(pi => pi.Id == plannedInspectionId))
                        PlannedInspections.Remove(PlannedInspections.First(pi => pi.Id == plannedInspectionId));
                }
                catch (QuestionHasAnswersException)
                {
                    OpenValidationPopup(
                        $"De inspectie voor {plannedInspection.Employee.Name} kon niet worden verwijderd omdat er een vraag met antwoorden in zit.");
                }
                catch (InvalidDataException)
                {
                    OpenValidationPopup(
                        $"De inspectie voor {plannedInspection.Employee.Name} kon niet worden verwijderd omdat de ingevulde gegevens niet voldoen.");
                }
            }

            OriginalPlannedInspectionIds = keptPlannedInspectionIds;
            RaisePropertyChanged(nameof(PlannedInspections));

            if (!PopupIsOpen)
                _navigationService.NavigateTo("FestivalInfo", _festival.Id);
        }

        private async void Save()
        {
            try
            {
                await _inspectionService.ProcessPlannedInspections(PlannedInspections, SelectedQuestionnaire, Instructions);

                foreach (int originalPlannedInspectionId in OriginalPlannedInspectionIds)
                {
                    if (PlannedInspections.All(pi => pi.Id != originalPlannedInspectionId))
                        await _inspectionService.RemoveInspection(originalPlannedInspectionId, "Inspectie geannuleerd");
                }

                _navigationService.NavigateTo("FestivalInfo", Festival.Id);
            }
            catch (InvalidDataException)
            {
                OpenValidationPopup("De ingevoerde data klopt niet of is involledig.");
            }
            catch (Exception e)
            {
                OpenValidationPopup($"Er is een fout opgetreden bij het opslaan van de inspectie ({e.GetType()})");
            }
        }
    }
}