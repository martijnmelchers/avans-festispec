using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Festivals
{
    public class FestivalListViewModel
    {
        private bool Filter(object item)
        {
            return string.IsNullOrEmpty(Search) ||
                   ((Festival) item).FestivalName.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public ICollectionView Festivals { get; set; }

        private string _search;

        public string Search 
        { 
            get => _search; 
            set
            {
                _search = value;

                Festivals.Filter += Filter;
            }
        }

        public ICommand OpenFestivalCommand { get; set; }

        private readonly IFrameNavigationService _navigationService;

        public FestivalListViewModel(IFrameNavigationService navigationService, IFestivalService festivalService)
        {
            _navigationService = navigationService;

            OpenFestivalCommand = new RelayCommand<Festival>(OpenFestival);
            Festivals = (CollectionView)CollectionViewSource.GetDefaultView(festivalService.GetFestivals());
            Festivals.Filter = Filter;
            
            festivalService.Sync();
        }

        private void OpenFestival(Festival festival)
        {
            _navigationService.NavigateTo("FestivalInfo", festival.Id);
        }
    }
}
