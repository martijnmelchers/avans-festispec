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

        public ObservableCollection<PointItem> Points { get; } = new ObservableCollection<PointItem>();

        private GoogleMapsService _googleMapsService;
        private IFestivalService _festivalService;
        private IFrameNavigationService _navigationService;

        public MapViewModel(IFrameNavigationService navigationService, GoogleMapsService googleMapsService, IFestivalService festivalService) {
            _googleMapsService = googleMapsService;
            _festivalService = festivalService;
            _navigationService = navigationService;

            var festivals = _festivalService.GetFestivals();

            foreach( var festival in festivals)
            {
                Points.Add(new PointItem()
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

        public void Navigate(string DestinationView, object DestinationParameter)
        {
            _navigationService.NavigateTo("FestivalInfo", 1);
        }


        private void FilterPoints()
        {
            // Check which items are checked and add them to the points list.
        }

    }
}
