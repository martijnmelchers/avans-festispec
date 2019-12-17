using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using Festispec.DomainServices.Services;
namespace Festispec.UI.ViewModels
{
    class MapViewModel : ViewModelBase
    {
        private GoogleMapsService _googleMapsService;

        public MapViewModel(GoogleMapsService googleMapsService) {
            _googleMapsService = googleMapsService;

            Saus();
        }

        private async void Saus()
        {
            var image = await _googleMapsService.GenerateStaticMap();
        }
    }
}
