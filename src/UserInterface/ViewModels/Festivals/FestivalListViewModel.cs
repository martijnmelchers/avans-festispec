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
        private readonly IFrameNavigationService _navigationService;

        public FestivalListViewModel(IFrameNavigationService navigationService, IFestivalService festivalService)
        {
            _navigationService = navigationService;

            OpenFestivalCommand = new RelayCommand<int>(OpenFestival);
            Festivals = (CollectionView) CollectionViewSource.GetDefaultView(festivalService.GetFestivals());
            Festivals.Filter = Filter;

            festivalService.Sync();
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

        private bool Filter(object item)
        {
            if (string.IsNullOrEmpty(Search))
                return true;
            return ((Festival) item).FestivalName.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void OpenFestival(int festivalId)
        {
            _navigationService.NavigateTo("FestivalInfo", festivalId);
        }
    }
}