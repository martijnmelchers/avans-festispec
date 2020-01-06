using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    public class FestivalListViewModel
    {
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(Search))
                return true;
            else
                return ((item as Models.Festival).FestivalName.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private ICollectionView _festivals { get; set; }

        public ICollectionView Festivals { get => _festivals; set => _festivals = value; }

        private string _search { get; set; }

        public string Search { get => _search; 
            set
            {
                _search = value;

                Festivals.Filter += Filter;
            }
        }

        private IFestivalService _festivalService;

        public ICommand OpenFestivalCommand { get; set; }

        public ICommand EditFestivalCommand { get; set; }

        private IFrameNavigationService _navigationService;

        public bool CanEdit { get; }

        public FestivalListViewModel(IFrameNavigationService navigationService, IFestivalService festivalService, OfflineService offlineService)
        {
            _festivalService = festivalService;
            _navigationService = navigationService;

            EditFestivalCommand = new RelayCommand<Festival>(EditFestival);
            OpenFestivalCommand = new RelayCommand<Festival>(OpenFestival);
            Festivals = (CollectionView)CollectionViewSource.GetDefaultView(_festivalService.GetFestivals());
            Festivals.Filter = new Predicate<object>(Filter);

            CanEdit = offlineService.IsOnline;
            
            festivalService.Sync();
        }

        private void OpenFestival(Festival festival)
        {
            _navigationService.NavigateTo("FestivalInfo", festival.Id);
        }

        public void EditFestival(Festival festival)
        {
            _navigationService.NavigateTo("UpdateFestival", festival.Id);
        }
    }
}
