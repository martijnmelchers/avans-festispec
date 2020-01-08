using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    class UpdateFestivalViewModel : ViewModelBase
    {
        public Festival Festival { get; set; }
        public ICommand UpdateFestivalCommand { get; set; }

        private readonly IFestivalService _festivalService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IGoogleMapsService _googleService;

        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }
        public UpdateFestivalViewModel(IFrameNavigationService navigationService, IFestivalService festivalService, IGoogleMapsService googleMapsService)
        {
            _festivalService = festivalService;
            _navigationService = navigationService;
            _googleService = googleMapsService;
            Festival = _festivalService.GetFestival((int)_navigationService.Parameter);
            UpdateFestivalCommand = new RelayCommand(UpdateFestival);

            #region Google Search
            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
            #endregion
        }
        public async void UpdateFestival()
        {            
            try
            {
                await _festivalService.UpdateFestival(Festival);
                _navigationService.NavigateTo("FestivalInfo", Festival.Id);
            }
            catch (InvalidAddressException)
            {
                MessageBox.Show("Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding festival. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Google Search
        public ObservableCollection<Prediction> Suggestions { get; set; }
        public string SearchQuery { get; set; }
        public string CurrentAddress { get; set; }

        public async void Search()
        {
            try
            {
                Suggestions = new ObservableCollection<Prediction>(await _googleService.GetSuggestions(SearchQuery));
                RaisePropertyChanged(nameof(Suggestions));
            }
            catch (GoogleMapsApiException)
            {
                MessageBox.Show("Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
            catch (GoogleZeroResultsException)
            {
                MessageBox.Show("Er zijn geen resultaten gevonden voor je zoekopdracht, wijzig je opdracht en probeer het opnieuw.");
            }


        }

        public async void Select(string id)
        {
            try
            {
                var address = await _googleService.GetAddress(id);
                Festival.Address = address;
                CurrentAddress = $"Geselecteerde adres: {Festival.Address.ToString()}";
                RaisePropertyChanged(nameof(CurrentAddress));
            }
            catch (GoogleMapsApiException)
            {
                MessageBox.Show("Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
        }

        #endregion
    }
}
