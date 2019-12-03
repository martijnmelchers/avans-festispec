using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    class CreateFestivalViewModel
    {
        private IFestivalService _festivalService;
        public Festival Festival { get; set; }
        public Customer Customer { get; set; }
        public ICommand CreateFestivalCommand { get; set; }
        public CreateFestivalViewModel(IFestivalService festivalService)
        {
            Festival = new Festival();
            _festivalService = festivalService;
            CreateFestivalCommand = new RelayCommand(CreateFestival);
        }
        public void CreateFestival()
        {
            
        }
    }
}
