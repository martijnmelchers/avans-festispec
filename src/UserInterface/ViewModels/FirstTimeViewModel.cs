using Festispec.DomainServices.Interfaces;
using System.Windows.Navigation;

namespace Festispec.UI.ViewModels
{
    public class FirstTimeViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        public FirstTimeViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public void Nigga()
        {

        }
    }
}
