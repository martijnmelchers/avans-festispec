﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Festivals
{
    internal class UpdateFestivalViewModel : BaseValidationViewModel
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
            try
            {
                await _festivalService.UpdateFestival(Festival);
                _festivalService.Sync();
                _navigationService.NavigateTo("FestivalInfo", Festival.Id);
            }
            catch (InvalidDataException)
            {
                OpenValidationPopup($"Ingevoerde data incorrect.");
            }
            catch (InvalidAddressException)
            {
                OpenValidationPopup("Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.");
            }
            catch (EndDateEarlierThanStartDateException)
            {
                OpenValidationPopup("De einddatum moet later zijn dan de startdatum");
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
                OpenValidationPopup($"Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator.");
            }
            catch (GoogleZeroResultsException)
            {
                OpenValidationPopup($"Er zijn geen resultaten gevonden voor je zoekopdracht, wijzig je opdracht en probeer het opnieuw.");
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
                OpenValidationPopup($"Er is een fout opgetreden tijdens het communiceren met Google Maps.Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator.");
            }
        }
        #endregion
    }
}