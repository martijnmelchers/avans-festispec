using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Windows.Media;
using MapControl;
using System.Collections.ObjectModel;
using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{

    public class PointItem : ViewModelBase
    {
        public MapViewModel Parent;

        public string DestinationView { get; set; }

        public object DestinationParameter { get; set; }

        public SolidColorBrush DotColor { get; set; }

        public ICommand LabelCommand { get; set; }

        public PointItem()
        {
            LabelCommand = new RelayCommand(Navigate);
        }

        private void Navigate()
        {
            Parent.Navigate(DestinationView, DestinationParameter);
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
                RaisePropertyChanged(nameof(Location));
            }
        }
    }


    public class MapViewModel : ViewModelBase
    {

        public ObservableCollection<PointItem> Points { get; set; } = new ObservableCollection<PointItem>();
        private List<PointItem> CachePoints = new List<PointItem>();

        public bool MedewerkerChecked { get; set; } = true;
        public bool KlantChecked      { get; set; } = true;
        public bool FestivalChecked   { get; set; } = true;

        
        private IFestivalService _festivalService;
        private IFrameNavigationService _navigationService;
        private ICustomerService _customerService;
        private IEmployeeService _employeeService;

        public ICommand CheckboxCheckedCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public MapViewModel(
            IFrameNavigationService navigationService,
            IFestivalService festivalService,
            ICustomerService customerService,
            IEmployeeService employeeService
        )
        {
            _festivalService   = festivalService;
            _navigationService = navigationService;
            _customerService   = customerService;
            _employeeService   = employeeService;

            CheckboxCheckedCommand = new RelayCommand(FilterPoints);
            BackCommand = new RelayCommand(Back);

            LoadPoints();
            FilterPoints();
        }

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
            {
                CachePoints.Add(new PointItem()
                {
                    Name = customer.CustomerName,
                    Location = new Location(customer.Address.Latitude, customer.Address.Longitude),
                    DestinationParameter = customer.Id,
                    DestinationView = "EditCustomer",
                    Parent = this,
                    DotColor = new SolidColorBrush(Colors.Blue)
                });
            }
        }

        private void LoadFestivals()
        {
            var festivals = _festivalService.GetFestivals();

            foreach (var festival in festivals)
            {
                CachePoints.Add(new PointItem()
                {
                    Name = festival.FestivalName,
                    Location = new Location(festival.Address.Latitude, festival.Address.Longitude),
                    DestinationParameter = festival.Id,
                    DestinationView = "FestivalInfo",
                    Parent = this,
                    DotColor = new SolidColorBrush(Colors.Red)
                });
            }
        }

        private void LoadEmployees()
        {
            var employees = _employeeService.GetAllEmployees();

            foreach (var employee in employees)
            {
                CachePoints.Add(new PointItem()
                {
                    Name = employee.Name.ToString(),
                    Location = new Location(employee.Address.Latitude, employee.Address.Longitude),
                    DestinationParameter = employee.Id,
                    DestinationView = "EmployeeInfo",
                    Parent = this,
                    DotColor = new SolidColorBrush(Colors.Aqua)
                });
            }
        }

        public void Navigate(string DestinationView, object DestinationParameter)
        {
            _navigationService.NavigateTo(DestinationView, DestinationParameter);
        }


        private void FilterPoints()
        {
            Points.Clear();
            // Check which items are checked and add them to the points list.
            foreach(var point in CachePoints)
            {
                switch (point.DestinationView)
                {
                    case "CustomerInfo":
                        if (KlantChecked)
                            Points.Add(point);
                    break;

                    case "FestivalInfo":
                        if (FestivalChecked)
                            Points.Add(point);
                    break;

                    case "EmployeeInfo":
                        if (MedewerkerChecked)
                            Points.Add(point);
                    break;

                    default:
                    break;
                }
            }

        }

    }
}
