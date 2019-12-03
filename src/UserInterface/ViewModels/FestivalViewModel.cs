using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    class FestivalViewModel
    {
        private IFestivalService _festivalService;
        public Festival Festival { get; set; }
        public string FestivalLocation => Festival.Address.StreetName + ", " + Festival.Address.City;
        public string FestivalData => Festival.OpeningHours.StartDate.ToString("dd/MM/yyyy") + " - " + Festival.OpeningHours.EndDate.ToString("dd/MM/yyyy");
        public string FestivalTimes => Festival.OpeningHours.StartTime.ToString(@"hh\:mm") + " - " + Festival.OpeningHours.EndTime.ToString(@"hh\:mm");
        public ICommand RemoveFestivalCommand { get; set; }
        public FestivalViewModel(IFestivalService festivalService)
        {
            _festivalService = festivalService;
            //change once festivallist is done
            Festival = _festivalService.GetFestival(1);
            RemoveFestivalCommand = new RelayCommand(RemoveFestival);
        }

        public async void RemoveFestival()
        {
            try
            {
                await _festivalService.RemoveFestival(Festival.Id);
            }
            catch(Exception e)
            {
                MessageBox.Show($"An error occured while removing festival with the id: {Festival.Id}. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }  
        }
    }
}
