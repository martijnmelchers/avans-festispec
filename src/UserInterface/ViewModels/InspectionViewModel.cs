using Festispec.DomainServices.Services;
using Festispec.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Festispec.DomainServices.Interfaces;

namespace Festispec.UI.ViewModels
{
    class InspectionViewModel : ViewModelBase
    {
        public Festival Festival { get; set; }
        ICommand CheckBoxCommand { get; set; }
        ICommand SaveCommand { get; set; }
        private IInspectionService _inspectionService;

        //public PlannedInspection plannedInspection { get; set; }

        //public List<Employee> Employees
        //{
        //    get { return new InspectionService(new Models.EntityMapping.FestispecContext()).GetEmployees(); }
        //}

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(Search))
                return true;
            else
                return ((item as Employee).Name.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private ICollectionView _employees { get; set; }
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
        public InspectionViewModel(IInspectionService inspectionService)
        {
            _inspectionService = inspectionService;
            CheckBoxCommand = new RelayCommand<Employee>(CheckBox);
            SaveCommand = new RelayCommand(Save);
            Employees = (CollectionView)CollectionViewSource.GetDefaultView(new InspectionService(new Models.EntityMapping.FestispecContext()).GetEmployees());
            Employees.Filter = new Predicate<object>(Filter);
            Festival = new Festival()
            {
                FestivalName = "test naam",
                Address = new Address()
                {
                    StreetName = "Straat naam",
                    City = "Biddingshuizen"
                },
                OpeningHours = new OpeningHours()
                {
                    StartTime = new DateTime(2019, 1, 1),
                    EndTime = new DateTime(2019, 1, 3)
                },
                Questionnaires = new List<Questionnaire>()
                {
                    new Questionnaire(){ },
                    new Questionnaire(){ },
                    new Questionnaire(){ }
                }
            };
        }

        public List<DateTime> GetDateOptions
        {
            get
            {
                var dateOptions = new List<DateTime>();
                foreach (DateTime day in EachDay(Festival.OpeningHours.StartTime, Festival.OpeningHours.EndTime))
                {
                    dateOptions.Add(day);
                }

                return dateOptions;
            }
        }
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
        ObservableCollection<Employee> EmployeesToAdd { get; set; }
        ObservableCollection<Employee> EmployeesToRemove { get; set; }
        public void CheckBox(Employee employee)
        {
            if (!Festival.PlannedInspections.Any(e => e.Employee == employee) || !EmployeesToAdd.Contains(employee))
            {
                EmployeesToAdd.Add(employee);
            }
            else if (EmployeesToAdd.Contains(employee))
            {
                EmployeesToAdd.Remove(employee);
            }
            else if(Festival.PlannedInspections.Any(e=>e.Employee == employee))
            {
                EmployeesToRemove.Add(employee);
            }
        }
        private  Questionnaire _questionnaire { get; set; }
        public  Questionnaire Questionnaire
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
        public void Save()
        {
            EmployeesToAdd.ToList().ForEach(e => _inspectionService.CreatePlannedInspection(Festival, Questionnaire));
        }

    }
}
