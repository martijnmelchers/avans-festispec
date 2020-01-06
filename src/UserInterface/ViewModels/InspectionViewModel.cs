using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    internal class InspectionViewModel : ViewModelBase
    {
        public Festival Festival { get; set; }
        public ICommand CheckBoxCommand { get; set; }
        public ICommand AddEmployee { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        private IInspectionService _inspectionService;
        private IFrameNavigationService _navigationService;
        private IFestivalService _festivalService;

        private DateTime _originalStartTime { get; set; }

        private bool Filter(object item)
        {            

            foreach(PlannedEvent p in (item as Employee).PlannedEvents)
            {
                if (p.StartTime.Date == SelectedDate.Date)
                    return false;
            }

            if (String.IsNullOrEmpty(Search))
                return true;
            else
                return ((item as Employee).Name.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private ICollectionView _employees { get; set; }
        private List<PlannedInspection> _plannedInspections { get; set; }

        public ICollectionView Employees
        {
            get
            {
                return _employees;
            }
            set
            {
                _employees = value;
            }
        }

        private string _search { get; set; }

        public string Search
        {
            get
            {
                return _search;
            }
            set
            {
                _search = value;

                Employees.Filter += Filter;
            }
        }

        public InspectionViewModel(IInspectionService inspectionService, IFestivalService festivalService, IFrameNavigationService navigationService)
        {
            _inspectionService = inspectionService;
            _navigationService = navigationService;
            _festivalService = festivalService;
            CheckBoxCommand = new RelayCommand<Employee>(CheckBox);
            SaveCommand = new RelayCommand(Save);
            ReturnCommand = new RelayCommand(Return);
            AddEmployee = new RelayCommand(Save);
            
            _plannedInspections = new List<PlannedInspection>();
           
            EmployeesToAdd = new ObservableCollection<Employee>();
            EmployeesToRemove = new ObservableCollection<Employee>();
            EmployeesAdded = new ObservableCollection<Employee>(); 
            Task.Run(async () => await Initialize(_navigationService.Parameter));
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
                
                _plannedInspections = await _inspectionService.GetPlannedInspections(temp.Festival, temp.StartTime);
                _plannedInspections.ForEach(p => EmployeesAdded.Add(p.Employee));
                //RaisePropertyChanged(nameof(EmployeesAdded));
            }
            else if(parameter.FestivalId > 0)
            {
                Festival = await _festivalService.GetFestivalAsync(parameter.FestivalId);
            }

            if (Festival == null)
            {
                throw new System.Exception();
            }

            _originalStartTime = _startTime;
                RaisePropertyChanged(nameof(Festival));
                RaisePropertyChanged(nameof(GetDateOptions));
                RaisePropertyChanged(nameof(Questionnaire));
                RaisePropertyChanged(nameof(StartTime));
                RaisePropertyChanged(nameof(EndTime));
                RaisePropertyChanged(nameof(SelectedDate));
                RaisePropertyChanged(nameof(CheckBox));
            Employees = (CollectionView)CollectionViewSource.GetDefaultView(_inspectionService.GetEmployees());
                RaisePropertyChanged(nameof(Employees));
            Employees.Filter = new Predicate<object>(Filter);
        }

        public List<DateTime> GetDateOptions
        {
            get
            {
                if (Festival == null)
                    return new List<DateTime>();

                var dateOptions = new List<DateTime>();
                foreach (DateTime day in EachDay(Festival.OpeningHours.StartDate, Festival.OpeningHours.EndDate))
                {
                    dateOptions.Add(day);
                }

                return dateOptions;
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
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
                }
                catch (Exception)
                {
                }
            }
        }

        public ObservableCollection<Employee> EmployeesToAdd { get; set; }
        public ObservableCollection<Employee> EmployeesToRemove { get; set; }
        public ObservableCollection<Employee> EmployeesAdded { get; set; }

        public void CheckBox(Employee employee)
        {
            //if (!Festival.PlannedInspections.Any(e => e.Employee == employee) || !EmployeesToAdd.Contains(employee))
            //{
            //    EmployeesToAdd.Add(employee);
            //}
            //else if (EmployeesToAdd.Contains(employee))
            //{
            //    EmployeesToAdd.Remove(employee);
            //}
            //else if (Festival.PlannedInspections.Any(e => e.Employee == employee))
            //{
            //    EmployeesToRemove.Add(employee);
            //}

            if (!EmployeesToAdd.Any(e => e.Id == employee.Id) && !EmployeesAdded.Any(e => e.Id == employee.Id))
                EmployeesToAdd.Add(employee);
            else if (EmployeesToAdd.Any(e => e.Id == employee.Id))
                EmployeesToAdd.Remove(employee);
            else if (EmployeesAdded.Any(e => e.Id == e.Id))
            {
                EmployeesToRemove.Add(employee);
                EmployeesAdded.Remove(employee);
            }
        }

        private Questionnaire _questionnaire { get; set; }

        public Questionnaire Questionnaire
        {
            get
            {
                return _questionnaire;
            }
            set
            {
                _questionnaire = value;
            }
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
                //try
                //{
                    await _inspectionService.CreatePlannedInspection(Festival, Questionnaire, _startTime, _endTime, "test", q);
                    
                //}
                //catch (Exception e)
                //{
                //    MessageBox.Show($"An error occured while adding a question. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
            EmployeesToAdd.Clear();
            foreach (Employee q in EmployeesToRemove)
            {
                try
                {
                    var plannedInspection = await _inspectionService.GetPlannedInspection(Festival, q, _originalStartTime);
                    await _inspectionService.RemoveInspection(plannedInspection.Id, "Slecht weer");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"An error occured while removing a question. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            EmployeesToRemove.Clear();
            await _inspectionService.SaveChanges();
            _navigationService.NavigateTo("FestivalInfo", Festival.Id);
        }
        private void Return()
        {
            _navigationService.NavigateTo("FestivalInfo", Festival.Id);
        }
    }
}