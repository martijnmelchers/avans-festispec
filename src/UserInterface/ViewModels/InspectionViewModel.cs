using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public InspectionViewModel(
            IInspectionService inspectionService,
            IFrameNavigationService navigationService,
            IGoogleMapsService googleService
        )
        {
            _inspectionService = inspectionService;
            _navigationService = navigationService;
            _googleService = googleService;

            CheckBoxCommand = new RelayCommand<AdvancedEmployee>(CheckBox);
            SaveCommand = new RelayCommand(Save);
            ReturnCommand = new RelayCommand(() => _navigationService.NavigateTo("FestivalInfo", Festival.Id));

            EmployeesToAdd = new ObservableCollection<Employee>();
            EmployeesToRemove = new ObservableCollection<Employee>();
            EmployeesAdded = new ObservableCollection<Employee>();

            Task.Run(() => Initialize(_navigationService.Parameter)).Wait();
        }

        public Festival Festival { get; set; }
        public ICommand CheckBoxCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ReturnCommand { get; set; }

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

        public ObservableCollection<Employee> EmployeesToAdd { get; }
        private ObservableCollection<Employee> EmployeesToRemove { get; }
        public ObservableCollection<Employee> EmployeesAdded { get; }

        public Questionnaire Questionnaire { get; set; }

        private bool Filter(object item)
        {
            Employee employee = (item as AdvancedEmployee).Employee;
            if (EmployeeHasNoPlannedInspection(employee) && EmployeeIsAvailable(employee))
            {
                if (string.IsNullOrEmpty(Search))
                    return true;
                return employee.Name.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }

        private bool EmployeeIsAvailable(Employee employee)
        {
            return employee.PlannedEvents.OfType<Availability>().All(item =>
                _startTime.Ticks < item.StartTime.Ticks
                && _endTime.Ticks < item.StartTime.Ticks || _startTime.Ticks > item.EndTime.Ticks
                && _endTime.Ticks > item.EndTime.Ticks);
        }

        private bool EmployeeHasNoPlannedInspection(Employee employee)
        {
            foreach (PlannedEvent item in employee.PlannedEvents)
                if (item is PlannedInspection)
                {
                    // check if new or edit
                    if (_originalStartTime == _startTime && _originalStartTime.Year > 100)
                        return true;

                    if ((_startTime.Ticks >= item.StartTime.Ticks || _endTime.Ticks >= item.StartTime.Ticks) &&
                        (_startTime.Ticks <= item.EndTime.Ticks || _endTime.Ticks <= item.EndTime.Ticks))
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
                _startTime = temp.StartTime;
                _endTime = temp.EndTime;
                Questionnaire = temp.Questionnaire;
                _selectedDate = temp.StartTime;

                List<PlannedInspection> existingPlannedInspections = await _inspectionService.GetPlannedInspections(temp.Festival.Id, temp.StartTime);
                existingPlannedInspections.ForEach(p => EmployeesAdded.Add(p.Employee));
            }
            else if (parameter.FestivalId > 0)
            {
                Festival = await _inspectionService.GetFestivalAsync(parameter.FestivalId);
            }

            if (Festival == null)
                throw new Exception();

            _originalStartTime = _startTime;
            RaisePropertyChanged(nameof(Festival));
            RaisePropertyChanged(nameof(GetDateOptions));
            RaisePropertyChanged(nameof(Questionnaire));
            RaisePropertyChanged(nameof(StartTime));
            RaisePropertyChanged(nameof(EndTime));
            RaisePropertyChanged(nameof(SelectedDate));
            RaisePropertyChanged(nameof(CheckBox));

            List<Employee> employees = _inspectionService.GetAllInspectors();
            var advancedEmployees = new List<AdvancedEmployee>();
            foreach (Employee employee in employees)
            {
                double distance = await _googleService.CalculateDistance(Festival.Address, employee.Address);

                advancedEmployees.Add(new AdvancedEmployee
                    {Employee = employee, Distance = $"{distance} km", DoubleDistance = distance});
            }

            Employees =
                (CollectionView) CollectionViewSource.GetDefaultView(advancedEmployees.OrderBy(e => e.DoubleDistance));
            RaisePropertyChanged(nameof(Employees));
            Employees.Filter = Filter;
            Employees.Filter += Filter;

            _inspectionService.Sync();
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (DateTime day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private void CheckBox(AdvancedEmployee advancedEmployee)
        {
            Employee employee = advancedEmployee.Employee;
            if (EmployeesToAdd.All(e => e.Id != employee.Id) && EmployeesAdded.All(e => e.Id != employee.Id))
            {
                EmployeesToAdd.Add(employee);
            }
            else if (EmployeesToAdd.Any(e => e.Id == employee.Id))
            {
                EmployeesToAdd.Remove(employee);
            }
            else if (EmployeesAdded.Any(e => e.Id == employee.Id))
            {
                EmployeesToRemove.Add(employee);
                EmployeesAdded.Remove(employee);
            }
        }

        private async void Save()
        {
            foreach (Employee q in EmployeesToAdd)
                try
                {
                    await _inspectionService.CreatePlannedInspection(Festival.Id, Questionnaire.Id, _startTime, _endTime,
                        "test", q.Id); // TODO replace this
                }
                catch (EntityExistsException)
                {
                    OpenValidationPopup("De ingevoerde data klopt niet of is involledig.");
                }
                catch (InvalidDataException)
                {
                    OpenValidationPopup("De ingevoerde data klopt niet of is involledig.");
                }
                catch (Exception)
                {
                    OpenValidationPopup("De ingevoerde data klopt niet of is involledig.");
                }

            EmployeesToAdd.Clear();
            foreach (Employee q in EmployeesToRemove)
                try
                {
                    PlannedInspection plannedInspection =
                        await _inspectionService.GetPlannedInspection(Festival, q, _originalStartTime);
                    await _inspectionService.RemoveInspection(plannedInspection.Id, "Niet meer nodig");
                }
                catch (QuestionHasAnswersException)
                {
                    OpenValidationPopup("De inspectie kan niet worden verwijderd omdat er een vraag met antwoorden in zit.");
                }
                catch (InvalidDataException)
                {
                    OpenValidationPopup("De ingevoerde data klopt niet of is involledig.");
                }

            EmployeesToRemove.Clear();
            await _inspectionService.SaveChanges();

            if (!PopupIsOpen) _navigationService.NavigateTo("FestivalInfo", Festival.Id);
        }
    }
}