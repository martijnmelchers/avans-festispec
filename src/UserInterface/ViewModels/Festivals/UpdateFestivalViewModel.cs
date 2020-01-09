using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    internal class UpdateFestivalViewModel : ViewModelBase
    {
        private readonly IFestivalService _festivalService;
        private readonly IGoogleMapsService _googleService;
        private readonly IFrameNavigationService _navigationService;

        public UpdateFestivalViewModel(IFrameNavigationService navigationService, IFestivalService festivalService,
            IGoogleMapsService googleMapsService)
        {
            _festivalService = festivalService;
            _navigationService = navigationService;
            _googleService = googleMapsService;
            Festival = _festivalService.GetFestival((int) _navigationService.Parameter);
            UpdateFestivalCommand = new RelayCommand(UpdateFestival);
            CancelCommand = new RelayCommand(() => _navigationService.NavigateTo("FestivalInfo", Festival.Id));

            #region Google Search

            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
            CurrentAddress = $"Huidige adres: {Festival.Address}";

            #endregion
        }

        public Festival Festival { get; set; }
        public ICommand UpdateFestivalCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }

        public async void UpdateFestival()
        {
            if (string.IsNullOrEmpty(CurrentAddress))
            {
                MessageBox.Show("Please select an address");
                return;
            }

            try
            {
                await _festivalService.UpdateFestival(Festival);
                _festivalService.Sync();
                _navigationService.NavigateTo("FestivalInfo", Festival.Id);
            }
            catch (QuestionHasAnswersException)
            {
                MessageBox.Show("De inspectie kan niet worden verwijderd omdat er een vraag met antwoorden in zit.",
                    "Kan inspectie niet verwijderen.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("De inspectie kan niet worden verwijderd omdat de ingevulde gegevens niet voldoen.",
                    "Kan inspectie niet verwijderen.", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Suggestions =
                    new ObservableCollection<Prediction>(
                        await _googleService.GetSuggestions(SearchQuery ?? string.Empty));
                RaisePropertyChanged(nameof(Suggestions));
            }
            catch (GoogleMapsApiException)
            {
                MessageBox.Show(
                    "Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
            catch (GoogleZeroResultsException)
            {
                MessageBox.Show(
                    "Er zijn geen resultaten gevonden voor je zoekopdracht, wijzig je opdracht en probeer het opnieuw.");
            }
        }

        public async void Select(string id)
        {
            try
            {
                Address address = await _googleService.GetAddress(id);
                Festival.Address = address;
                CurrentAddress = $"Geselecteerde adres: {Festival.Address}";
                RaisePropertyChanged(nameof(CurrentAddress));
            }
            catch (GoogleMapsApiException)
            {
                MessageBox.Show(
                    "Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
        }

        #endregion
    }
}