using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Festispec.UI.Exceptions;
using System.Collections.ObjectModel;
using Festispec.Models.Google;
using GalaSoft.MvvmLight;
using Festispec.Models.Exception;

namespace Festispec.UI.ViewModels
{
    class CreateFestivalViewModel : ViewModelBase
    {
        private IFestivalService _festivalService;
        public Festival Festival { get; set; }
        public ICommand CreateFestivalCommand { get; set; }

        private readonly IFrameNavigationService _navigationService;
        private readonly ICustomerService _customerService;
        private readonly IGoogleMapsService _googleService;

        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }

        public CreateFestivalViewModel(IFrameNavigationService navigationService, ICustomerService customerService, IFestivalService festivalService, IGoogleMapsService googleMapsService)
        {
            Festival = new Festival
            {
                OpeningHours = new OpeningHours(),
                Address = new Address()
            };
            _festivalService = festivalService;
            _customerService = customerService;
            _navigationService = navigationService;
            
            if (navigationService.Parameter == null || !(navigationService.Parameter is int customerId))
                throw new InvalidNavigationException();
            
            Festival.Customer = _customerService.GetCustomer(customerId);
            CreateFestivalCommand = new RelayCommand(CreateFestival);

            #region Google Search
            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
            #endregion
        }
        public async void CreateFestival()
        {
            if (String.IsNullOrEmpty(CurrentAddress))
            {
                MessageBox.Show("Please select an address");
                return;
            }

            try
            {
                await _festivalService.CreateFestival(Festival);
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
                Suggestions = new ObservableCollection<Prediction>(await _googleService.GetSuggestions(SearchQuery ?? string.Empty));
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
