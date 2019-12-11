using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    class UpdateFestivalViewModel
    {
        private IFestivalService _festivalService;
        public Festival Festival { get; set; }
        public ICommand UpdateFestivalCommand { get; set; }
        public string Suffix { get; set; }
        public List<string> CountryOptions
        {
            get
            {
                return new List<string>()
                {
                    "Nederland",
                    "België",
                    "Duitsland"
                };
            }
        }
        private IFrameNavigationService _navigationService;
        public UpdateFestivalViewModel(IFrameNavigationService navigationService, IFestivalService festivalService)
        {
            _festivalService = festivalService;
            _navigationService = navigationService;
            Festival = _festivalService.GetFestival((int)_navigationService.Parameter);
            UpdateFestivalCommand = new RelayCommand(UpdateFestival);
        }
        public async void UpdateFestival()
        {
            if (Festival.Address.Suffix != Suffix)
            {
                if (!string.IsNullOrEmpty(Suffix))
                {
                    Festival.Address.Suffix = Suffix;
                }
                else
                {
                    Festival.Address.Suffix = null;
                }
            }
            
            try
            {
                await _festivalService.SaveChangesToFestival(Festival);
                _navigationService.NavigateTo("FestivalInfo", Festival.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding festival. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
