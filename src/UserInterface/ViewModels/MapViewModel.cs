using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using Festispec.DomainServices.Services;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MapControl;
using System.Collections.ObjectModel;
using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows;

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

        public bool InspecteurChecked { get; set; } = true;
        public bool MedewerkerChecked { get; set; } = true;
        public bool KlantChecked      { get; set; } = true;
        public bool FestivalChecked   { get; set; } = true;

        
        private GoogleMapsService _googleMapsService;
        private IFestivalService _festivalService;
        private IFrameNavigationService _navigationService;
        private ICustomerService _customerService;

        public ICommand CheckboxCheckedCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public MapViewModel(
            IFrameNavigationService navigationService,
            GoogleMapsService googleMapsService,
            IFestivalService festivalService,
            ICustomerService customerService
        )
        {
            _googleMapsService = googleMapsService;
            _festivalService = festivalService;
            _navigationService = navigationService;
            _customerService = customerService;

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
            var festivals = _festivalService.GetFestivals();
            var customers = _customerService.GetAllCustomers();

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

            foreach(var customer in customers)
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
                    case "EditCustomer":
                        if (KlantChecked)
                            Points.Add(point);
                    break;

                    case "FestivalInfo":
                        if (FestivalChecked)
                            Points.Add(point);
                    break;

                    default:
                    break;
                }
            }

        }

    }
}
