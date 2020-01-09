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
    internal class CreateFestivalViewModel : BaseValidationViewModel
    {
        private readonly IGoogleMapsService _googleService;

        private readonly IFrameNavigationService _navigationService;
        private readonly IFestivalService _festivalService;
        private readonly int _customerId;

        public CreateFestivalViewModel(IFrameNavigationService navigationService, IFestivalService festivalService,
            IGoogleMapsService googleMapsService)
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

            _customerId = customerId;

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
            try
            {
                await _festivalService.CreateFestival(Festival, _customerId);
                _festivalService.Sync();
                _navigationService.NavigateTo("FestivalInfo", Festival.Id);
            }
            catch (InvalidAddressException)
            {
                OpenValidationPopup("Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.");
            }
            catch (Exception e)
            {
                OpenValidationPopup($"An error occured while adding festival. The occured error is: {e.GetType()}");
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
                OpenValidationPopup("Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
            catch (GoogleZeroResultsException)
            {
                OpenValidationPopup("Er zijn geen resultaten gevonden voor je zoekopdracht, wijzig je opdracht en probeer het opnieuw.");
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
                OpenValidationPopup("Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
        }

        #endregion
    }
}