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
    class CreateFestivalViewModel
    {
        private IFestivalService _festivalService;
        public Festival Festival { get; set; }
        public ICommand CreateFestivalCommand { get; set; }
        public List<string> CountryOptions { get; set; }
        public CreateFestivalViewModel(IFestivalService festivalService)
        {
            AddCountries();
            //deze regel is om te testen, later weghalen
            Festival = new Festival
            {
                OpeningHours = new OpeningHours(),
                Address = new Address()
            };
            _festivalService = festivalService;
            Festival.Customer = _festivalService.GetCustomer(1);
            CreateFestivalCommand = new RelayCommand(CreateFestival);
        }
        public async void CreateFestival()
        {
            try
            {
                await _festivalService.CreateFestival(Festival);
            }
            catch(Exception e)
            {
                MessageBox.Show($"An error occured while adding festival. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddCountries()
        {
            CountryOptions = new List<string>()
            {
                "Nederland",
                "België",
                "Duitsland"
            };
        }
    }
}
