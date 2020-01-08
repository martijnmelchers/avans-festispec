using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MapControl;

namespace Festispec.UI.ViewModels
{
    public class PointItem : ViewModelBase
    {
        private Location location;

        private string name;
        public MapViewModel Parent;

        public PointItem()
        {
            LabelCommand = new RelayCommand(Navigate);
        }

        public string DestinationView { get; set; }

        public object DestinationParameter { get; set; }

        public SolidColorBrush DotColor { get; set; }

        public ICommand LabelCommand { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public Location Location
        {
            get => location;
            set
            {
                location = value;
                RaisePropertyChanged(nameof(Location));
            }
        }

        private void Navigate()
        {
            Parent.Navigate(DestinationView, DestinationParameter);
        }
    }
}