using GalaSoft.MvvmLight;

namespace Festispec.UI.ViewModels
{
    public abstract class BaseValidationViewModel : ViewModelBase
    {
        private bool _popupIsOpen;
        private string _validationError;
        private string _caption;

        public bool PopupIsOpen
        {
            get => _popupIsOpen;
            set { _popupIsOpen = value; RaisePropertyChanged(); }
        }

        public string ValidationError
        {
            get => _validationError;
            set { _validationError = value; RaisePropertyChanged(); }
        }
    }
}