using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.UI.ViewModels
{
    class FestivalViewModel
    {
        private IFestivalService _festivalService;
        private Festival _festival;
        public string FestivalHeader { get => "Informatie " + _festival.FestivalName;}
        public FestivalViewModel(IFestivalService festivalService)
        {
            _festivalService = festivalService;
            _festival = _festivalService.GetFestival(1);
        }
    }
}
