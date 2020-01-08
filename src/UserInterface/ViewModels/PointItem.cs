using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MapControl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

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
}
