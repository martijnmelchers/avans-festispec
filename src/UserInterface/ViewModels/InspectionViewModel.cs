using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    internal class InspectionViewModel : BaseDeleteCheckViewModel
    {
        public Festival Festival { get; set; }
        public ICommand CheckBoxCommand { get; set; }
        public ICommand AddEmployee { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        private readonly IInspectionService _inspectionService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IFestivalService _festivalService;
        private readonly IEmployeeService _employeeService;
        private readonly IGoogleMapsService _googleService;

        private DateTime _originalStartTime { get; set; }

        private bool Filter(object item)
        {
            var employee = (item as AdvancedEmployee).Employee;
            if (EmployeeHasNoPlannedInspection(employee) && EmployeeIsAvailable(employee))
            {
                if (string.IsNullOrEmpty(Search))
                    return true;
                else
                    return (employee.Name.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0);
            }
                return false;

        }

        private bool EmployeeIsAvailable(Employee employee)
        {
            foreach (var item in employee.PlannedEvents)
            {
                if (item is Availability)
                {
                    if ((_startTime.Ticks < item.StartTime.Ticks && _endTime.Ticks < item.StartTime.Ticks) || (_startTime.Ticks > item.EndTime.Ticks && _endTime.Ticks > item.EndTime.Ticks))
                    {
                        //check next one
                    }
                    else
                        return false;
                }
            }
            return true;
        }

        private bool EmployeeHasNoPlannedInspection(Employee employee)
        {
            foreach (var item in employee.PlannedEvents)
            {
                if (item is PlannedInspection)
                {
                    // check if new or edit
                    if (_originalStartTime == _startTime && _originalStartTime.Year > 100)
                        return true;
                    else
                    {
                        if ((_startTime.Ticks < item.StartTime.Ticks && _endTime.Ticks < item.StartTime.Ticks) || (_startTime.Ticks > item.EndTime.Ticks && _endTime.Ticks > item.EndTime.Ticks))
                        {
                            //checked next one
                        }
                        else
                            return false;
                    }
                }
            }

            return true;
        }

        private ICollectionView _employees { get; set; }
        private List<PlannedInspection> _plannedInspections { get; set; }

        public ICollectionView Employees
        {
            get => _employees;
            set => _employees = value;
        }

        private string _search { get; set; }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                Employees.Filter += Filter;
            }
        }

        public InspectionViewModel(IInspectionService inspectionService, IFestivalService festivalService, IFrameNavigationService navigationService, IEmployeeService employeeService, IGoogleMapsService googleService)
        {
            _inspectionService = inspectionService;
            _navigationService = navigationService;
            _festivalService = festivalService;
            _employeeService = employeeService;
            _googleService = googleService;

            CheckBoxCommand = new RelayCommand<AdvancedEmployee>(CheckBox);
            SaveCommand = new RelayCommand(Save);
            ReturnCommand = new RelayCommand(Return);
            AddEmployee = new RelayCommand(Save);

            _plannedInspections = new List<PlannedInspection>();

            EmployeesToAdd = new ObservableCollection<Employee>();
            EmployeesToRemove = new ObservableCollection<Employee>();
            EmployeesAdded = new ObservableCollection<Employee>();
            Task.Run(() => Initialize(_navigationService.Parameter)).Wait();
        }

        private async Task Initialize(dynamic parameter)
        {
            if (parameter.PlannedInspectionId > 0)
            {
                PlannedInspection temp = await _inspectionService.GetPlannedInspection(parameter.PlannedInspectionId);
                Festival = await _festivalService.GetFestivalAsync(temp.Festival.Id);
                _startTime = temp.StartTime;
                _endTime = temp.EndTime;
                Questionnaire = temp.Questionnaire;
                _selectedDate = temp.StartTime;

                _plannedInspections = await _inspectionService.GetPlannedInspections(temp.Festival, temp.StartTime);
                _plannedInspections.ForEach(p => EmployeesAdded.Add(p.Employee));
            }
            else if (parameter.FestivalId > 0)
                Festival = await _festivalService.GetFestivalAsync(parameter.FestivalId);

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

            var employees = _employeeService.GetAllInspectors();
            var advancedEmployees = new List<AdvancedEmployee>();
            foreach (Employee employee in employees)
            {

                    var distance = await _googleService.CalculateDistance(Festival.Address, employee.Address);

                    advancedEmployees.Add(new AdvancedEmployee() { Employee = employee, Distance = $"{distance} Km", DoubleDistance = distance });

            }
            _employees = (CollectionView)CollectionViewSource.GetDefaultView(advancedEmployees.OrderBy(e => e.DoubleDistance));
            RaisePropertyChanged(nameof(Employees));
            Employees.Filter = new Predicate<object>(Filter);
            Employees.Filter += Filter;
            
            _inspectionService.Sync();
        }

        public List<DateTime> GetDateOptions
        {
            get
            {
                if (Festival == null)
                    return new List<DateTime>();

                return EachDay(Festival.OpeningHours.StartDate, Festival.OpeningHours.EndDate).ToList();
            }
        }

        public DateTime SelectedDate
        {
            get => GetDateOptions.FirstOrDefault(e => e.Year == _startTime.Year && e.Month == _startTime.Month && e.Day == _startTime.Day);
            set
            {
                try
                {
                    _selectedDate = value;
                    DateTime outvar;
                    //Set start time
                    string dateString = $"{_selectedDate.Day}/{_selectedDate.Month}/{_selectedDate.Year} {_startTime.Hour}:{_startTime.Minute}";
                    bool isvalid = DateTime.TryParse(dateString, out outvar);
                    _startTime = outvar;
                    //Set end time
                    dateString = $"{_selectedDate.Day}/{_selectedDate.Month}/{_selectedDate.Year} {_endTime.Hour}:{_endTime.Minute}";
                    isvalid = DateTime.TryParse(dateString, out outvar);
                    _endTime = outvar;
                    Employees.Filter += Filter;
                }
                catch (Exception)
                {
                }
            }
        }

        public DateTime _selectedDate { get; set; }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private DateTime _startTime { get; set; }

        public string StartTime
        {
            get
            {
                return string.Format("{0}:{1}", _startTime.Hour.ToString().PadLeft(2, '0'), _startTime.Minute.ToString().PadLeft(2, '0'));
            }
            set
            {
                try
                {
                    string dateString = $"{_startTime.Day}/{_startTime.Month}/{_startTime.Year} {int.Parse(value.Substring(0, 2))}:{int.Parse(value.Substring(3, 2))}";
                    DateTime outvar;
                    bool isvalid = DateTime.TryParse(dateString, out outvar);
                    _startTime = outvar;
                    Employees.Filter += Filter;
                }
                catch (Exception)
                {
                }
            }
        }

        private DateTime _endTime { get; set; }

        public string EndTime
        {
            get
            {
                return string.Format("{0}:{1}", _endTime.Hour.ToString().PadLeft(2, '0'), _endTime.Minute.ToString().PadLeft(2, '0'));
            }
            set
            {
                try
                {
                    string dateString = $"{_endTime.Day}/{_endTime.Month}/{_endTime.Year} {int.Parse(value.Substring(0, 2))}:{int.Parse(value.Substring(3, 2))}";
                    DateTime outvar;
                    bool isvalid = DateTime.TryParse(dateString, out outvar);
                    _endTime = outvar;
                    Employees.Filter += Filter;
                }
                catch (Exception)
                {
                }
            }
        }

        public ObservableCollection<Employee> EmployeesToAdd { get; set; }
        public ObservableCollection<Employee> EmployeesToRemove { get; set; }
        public ObservableCollection<Employee> EmployeesAdded { get; set; }

        public void CheckBox(AdvancedEmployee advancedEmployee)
        {
            var employee = advancedEmployee.Employee;
            if (!EmployeesToAdd.Any(e => e.Id == employee.Id) && !EmployeesAdded.Any(e => e.Id == employee.Id))
                EmployeesToAdd.Add(employee);
            else if (EmployeesToAdd.Any(e => e.Id == employee.Id))
                EmployeesToAdd.Remove(employee);
            else if (EmployeesAdded.Any(e => e.Id == employee.Id))
            {
                EmployeesToRemove.Add(employee);
                EmployeesAdded.Remove(employee);
            }
        }

        private Questionnaire _questionnaire { get; set; }

        public Questionnaire Questionnaire
        {
            get => _questionnaire;

            set => _questionnaire = value;
        }

        public async void Save()
        {
            foreach (PlannedInspection p in _plannedInspections)
            {
                p.StartTime = _startTime;
                p.EndTime = _endTime;
                p.Questionnaire = Questionnaire;
            }

            foreach (Employee q in EmployeesToAdd)
            {
                try
                {
                    await _inspectionService.CreatePlannedInspection(Festival, Questionnaire, _startTime, _endTime, "test", q);
                }
                catch (EntityExistsException)
                {
                    ValidationError = "De ingevoerde data klopt niet of is involledig.";
                    PopupIsOpen = true;
                }
                catch (InvalidDataException)
                {
                    ValidationError = "De ingevoerde data klopt niet of is involledig.";
                    PopupIsOpen = true;
                }
                catch (Exception)
                {
                    ValidationError = "De ingevoerde data klopt niet of is involledig.";
                    PopupIsOpen = true;
                }
            }
            EmployeesToAdd.Clear();
            foreach (Employee q in EmployeesToRemove)
            {
                try
                {
                    var plannedInspection = await _inspectionService.GetPlannedInspection(Festival, q, _originalStartTime);
                    await _inspectionService.RemoveInspection(plannedInspection.Id, "Niet meer nodig");
                }
                catch (QuestionHasAnswersException)
                {
                    ValidationError = "De inspectie kan niet worden verwijderd omdat er een vraag met antwoorden in zit.";
                    PopupIsOpen = true;
                }
                catch (InvalidDataException)
                {
                    ValidationError = "De ingevoerde data klopt niet of is involledig.";
                    PopupIsOpen = true;
                }
            }
            EmployeesToRemove.Clear();
            await _inspectionService.SaveChanges();

            if (PopupIsOpen == false)
            {
            _navigationService.NavigateTo("FestivalInfo", Festival.Id);

            }
        }

        private void Return()
        {
            _navigationService.NavigateTo("FestivalInfo", Festival.Id);
        }
    }
}
