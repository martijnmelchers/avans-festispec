using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MapControl;
using Microsoft.Extensions.Configuration;

namespace Festispec.UI.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;


        private readonly IFestivalService _festivalService;
        private readonly IFrameNavigationService _navigationService;
        private readonly List<PointItem> _cachePoints = new List<PointItem>();

        public MapViewModel(
            IFrameNavigationService navigationService,
            IFestivalService festivalService,
            ICustomerService customerService,
            IEmployeeService employeeService,
            IConfiguration config
        )
        {
            _festivalService = festivalService;
            _navigationService = navigationService;
            _customerService = customerService;
            _employeeService = employeeService;

            CheckboxCheckedCommand = new RelayCommand(FilterPoints);
            BackCommand = new RelayCommand(Back);
            BingMapsTileLayer.ApiKey = config["ApiKeys:Bing"];

            LoadPoints();
            FilterPoints();
        }

        public ObservableCollection<PointItem> Points { get; set; } = new ObservableCollection<PointItem>();

        public bool EmployeeChecked { get; set; } = true;
        public bool CustomerChecked { get; set; } = true;
        public bool FestivalChecked { get; set; } = true;

        public ICommand CheckboxCheckedCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private void Back()
        {
            _navigationService.NavigateTo("HomePage");
        }

        private void LoadPoints()
        {
            LoadCustomers();
            LoadFestivals();
            LoadEmployees();
        }


        private void LoadCustomers()
        {
            var customers = _customerService.GetAllCustomers();

            foreach (var customer in customers)
                _cachePoints.Add(new PointItem
                {
                    Name = customer.CustomerName,
                    Location = new Location(customer.Address.Latitude, customer.Address.Longitude),
                    DestinationParameter = customer.Id,
                    DestinationView = "CustomerInfo",
                    Parent = this,
                    DotColor = new SolidColorBrush(Colors.Blue)
                });
        }

        private void LoadFestivals()
        {
            var festivals = _festivalService.GetFestivals();

            foreach (var festival in festivals)
                _cachePoints.Add(new PointItem
                {
                    Name = festival.FestivalName,
                    Location = new Location(festival.Address.Latitude, festival.Address.Longitude),
                    DestinationParameter = festival.Id,
                    DestinationView = "FestivalInfo",
                    Parent = this,
                    DotColor = new SolidColorBrush(Colors.Red)
                });
        }

        private void LoadEmployees()
        {
            var employees = _employeeService.GetAllEmployees();

            foreach (var employee in employees)
                _cachePoints.Add(new PointItem
                {
                    Name = employee.Name.ToString(),
                    Location = new Location(employee.Address.Latitude, employee.Address.Longitude),
                    DestinationParameter = employee.Id,
                    DestinationView = "EmployeeInfo",
                    Parent = this,
                    DotColor = new SolidColorBrush(Colors.Aqua)
                });
        }

        public void Navigate(string destinationView, object destinationParameter)
        {
            _navigationService.NavigateTo(destinationView, destinationParameter);
        }


        private void FilterPoints()
        {
            Points.Clear();
            // Check which items are checked and add them to the points list.
            foreach (var point in _cachePoints)
                switch (point.DestinationView)
                {
                    case "CustomerInfo":
                        if (CustomerChecked)
                            Points.Add(point);
                        break;

                    case "FestivalInfo":
                        if (FestivalChecked)
                            Points.Add(point);
                        break;

                    case "EmployeeInfo":
                        if (EmployeeChecked)
                            Points.Add(point);
                        break;
                }
        }
    }
}