using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    public class FestivalListViewModel
    {
        private readonly IFestivalService _festivalService;
        private readonly IFrameNavigationService _navigationService;

        public FestivalListViewModel(IFrameNavigationService navigationService, IFestivalService festivalService)
        {
            _festivalService = festivalService;
            _navigationService = navigationService;

            OpenFestivalCommand = new RelayCommand<int>(OpenFestival);
            Festivals = (CollectionView) CollectionViewSource.GetDefaultView(_festivalService.GetFestivals());
            Festivals.Filter = Filter;
        }

        private ICollectionView _festivals { get; set; }

        public ICollectionView Festivals
        {
            get => _festivals;
            set => _festivals = value;
        }

        private string _search { get; set; }

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

        public ICommand EditFestivalCommand { get; set; }

        private bool Filter(object item)
        {
            if (string.IsNullOrEmpty(Search))
                return true;
            return (item as Festival).FestivalName.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void OpenFestival(int festivalId)
        {
            _navigationService.NavigateTo("FestivalInfo", festivalId);
        }
    }
}