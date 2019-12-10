using Festispec.DomainServices.Interfaces;
using Festispec.Models;
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
        public UpdateFestivalViewModel(IFestivalService festivalService)
        {
            //deze regel is om te testen, later anders doen
            _festivalService = festivalService;
            Festival = _festivalService.GetFestival(1);
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
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding festival. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
