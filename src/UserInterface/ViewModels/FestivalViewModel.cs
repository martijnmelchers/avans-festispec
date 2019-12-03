using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    public class FestivalViewModel : ViewModelBase, IAsyncActivateable<int>
    {
        private readonly IFestivalService _festivalService;
        private Festival _festival;
        public Festival Festival
        {
            get { return _festival; }
            set { _festival = value; RaisePropertyChanged(nameof(Festival)); }
        }

        public List<Questionnaire> Questionnaires { get => new List<Questionnaire>() { new Questionnaire("Nigga", null) }; }
        public string FestivalLocation { get; set; }
        public string FestivalData { get; set; }
        public string FestivalTimes { get; set; }

        public ICommand RemoveFestivalCommand { get; set; }
        public FestivalViewModel(IFestivalService festivalService)
        {
            _festivalService = festivalService;
            //change once festivallist is done
            RemoveFestivalCommand = new RelayCommand(RemoveFestival);
        }

        public async Task Initialize(int id)
        {
            Festival = await _festivalService.GetFestival(1);
            FestivalLocation = Festival.Address.StreetName + ", " + Festival.Address.City;
            FestivalData = Festival.OpeningHours.StartTime.ToString("dd/MM/yyyy") + " - " + Festival.OpeningHours.EndTime.ToString("dd/MM/yyyy");
            FestivalTimes = Festival.OpeningHours.StartTime.ToString("HH/mm") + " - " + Festival.OpeningHours.EndTime.ToString("HH/mm");
        }

        public async void RemoveFestival()
        {
            try
            {
                await _festivalService.RemoveFestival(Festival.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while removing festival with the id: {Festival.Id}. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
