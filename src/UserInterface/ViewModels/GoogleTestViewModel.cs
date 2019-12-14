using Festispec.DomainServices.Services;
using Festispec.Models.Google;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    public class GoogleTestViewModel : ViewModelBase
    {
        public GoogleMapsService _googleService;

        public ObservableCollection<Prediction> Suggestions { get; set; }
        public Place Place { get; set; }
        public ICommand SearchCommand { get; set; }
        public RelayCommand<string> SelectCommand { get; set; }
        public string SearchQuery { get; set; }

        public GoogleTestViewModel(GoogleMapsService googleService)
        {
            _googleService = googleService;

            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
        }

        public async void Search()
        {
            try
            {
                Suggestions = new ObservableCollection<Prediction>(await _googleService.GetSuggestions(SearchQuery));
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR!");
            }

            RaisePropertyChanged(nameof(Suggestions));

        }

        public async void Select(string id)
        {
            try
            {
                Place = await _googleService.GetPlace(id);
                RaisePropertyChanged(nameof(Place));
                MessageBox.Show($"LAT: {Place.Geometry.Location.Latitude} LONG: {Place.Geometry.Location.Longitude}");
                var a = _googleService.PlaceToAddress(Place);
                MessageBox.Show(a.StreetName);
            } catch (Exception e)
            {
                MessageBox.Show("Error fetching place, try again!");
            }
        }


    }

}
