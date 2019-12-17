﻿using Festispec.DomainServices.Interfaces;
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
        private IInspectionService _inspectionService;
        private IFrameNavigationService _navigationService;
        private IFestivalService _festivalService;

        private DateTime _originalStartTime { get; set; }

        private bool Filter(object item)
        {
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
            AddEmployee = new RelayCommand(Save);
            Employees = (CollectionView)CollectionViewSource.GetDefaultView(_inspectionService.GetEmployees());
            _plannedInspections = new List<PlannedInspection>();
            Employees.Filter = new Predicate<object>(Filter);
            EmployeesToAdd = new ObservableCollection<Employee>();
            EmployeesToRemove = new ObservableCollection<Employee>();
            _originalStartTime = _startTime;
            //Festival = _inspectionService.GetFestival();
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
            }else if(parameter.FestivalId > 0)
            {
                Festival = await _festivalService.GetFestivalAsync(parameter.FestivalId);
            }
            
                RaisePropertyChanged(nameof(Festival));
                RaisePropertyChanged(nameof(GetDateOptions));
                RaisePropertyChanged(nameof(Employees));
                RaisePropertyChanged(nameof(Questionnaire));
                RaisePropertyChanged(nameof(StartTime));
                RaisePropertyChanged(nameof(EndTime));
                RaisePropertyChanged(nameof(SelectedDate));
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
                    string dateString = $"{_selectedDate.Day}/{_selectedDate.Month}/{_selectedDate.Year} {_startTime.Hour}:{_startTime.Minute}";
                    DateTime outvar;
                    bool isvalid = DateTime.TryParse(dateString, out outvar);
                    _startTime = outvar;
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
            else if (Festival.PlannedInspections.Any(e => e.Employee == employee))
            {
                EmployeesToRemove.Add(employee);
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
                //p.Questionnaire = Questionnaire;
            }

            foreach (Employee q in EmployeesToAdd)
            {
                //try
                //{
                    await _inspectionService.CreatePlannedInspection(Festival, Questionnaire, _startTime, _endTime, "test", q);
                    //EmployeesToAdd.Remove(q);
                //}
                //catch (Exception e)
                //{
                //    MessageBox.Show($"An error occured while adding a question. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
            //foreach (Employee q in EmployeesToRemove)
            //{
            //    try
            //    {
            //        await _inspectionService.RemoveInspection(0);
            //        EmployeesToRemove.Remove(q);
            //    }
            //    catch (Exception e)
            //    {
            //        MessageBox.Show($"An error occured while adding a question. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }
    }
}