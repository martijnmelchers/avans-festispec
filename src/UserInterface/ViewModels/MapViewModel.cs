using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using Festispec.DomainServices.Services;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Festispec.UI.ViewModels
{
    class MapViewModel : ViewModelBase
    {
        private GoogleMapsService _googleMapsService;
        public ImageSource Source { get; set; }

        private BitmapImage img;
        public MapViewModel(GoogleMapsService googleMapsService) {
            _googleMapsService = googleMapsService;
            SetUrl();
        }

        private async void SetUrl()
        {
            var url = await _googleMapsService.GenerateStaticMap();
            Source = new BitmapImage(new Uri(url));
        }
    }
}
