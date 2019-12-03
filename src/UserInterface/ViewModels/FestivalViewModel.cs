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
        public Festival Festival { get; set; }
        /*public string FestivalHeader { get => "Informatie " + _festival.FestivalName;}*/
        public FestivalViewModel(IFestivalService festivalService)
        {
            _festivalService = festivalService;
            Festival = _festivalService.GetFestival(1);
        }
    }
}
