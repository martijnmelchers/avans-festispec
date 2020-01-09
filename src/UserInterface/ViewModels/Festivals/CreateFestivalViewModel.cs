using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Exceptions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Festivals
{
    internal class CreateFestivalViewModel : ViewModelBase
    {
        private readonly IGoogleMapsService _googleService;

        private readonly IFrameNavigationService _navigationService;
        private readonly IFestivalService _festivalService;

        public CreateFestivalViewModel(IFrameNavigationService navigationService, ICustomerService customerService,
            IFestivalService festivalService, IGoogleMapsService googleMapsService)
        {
            Festival = new Festival
            {
                OpeningHours = new OpeningHours(),
                Address = new Address()
            };
            _festivalService = festivalService;
            _navigationService = navigationService;

            if (navigationService.Parameter == null || !(navigationService.Parameter is int customerId))
                throw new InvalidNavigationException();

            Festival.Customer = customerService.GetCustomer(customerId);
            CreateFestivalCommand = new RelayCommand(CreateFestival);

            #region Google Search

            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);

            #endregion
        }

        public Festival Festival { get; set; }
        public ICommand CreateFestivalCommand { get; set; }

        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }

        public async void CreateFestival()
        {
            if (string.IsNullOrEmpty(CurrentAddress))
            {
                MessageBox.Show("Please select an address");
                return;
            }

            try
            {
                await _festivalService.CreateFestival(Festival);
                //_festivalService.Sync();
                _navigationService.NavigateTo("FestivalInfo", Festival.Id);
            }
            catch (InvalidAddressException)
            {
                MessageBox.Show(
                    "Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding festival. The occured error is: {e.GetType()}",
                    $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
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