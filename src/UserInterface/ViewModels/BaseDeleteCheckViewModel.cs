using System.Windows.Input;

namespace Festispec.UI.ViewModels
{
    public abstract class BaseDeleteCheckViewModel : BaseValidationViewModel
    {
        private bool _deletePopupIsOpen;

        public bool DeletePopupIsOpen
        {
            get => _deletePopupIsOpen;
            private set
            {
                _deletePopupIsOpen = value;
                RaisePropertyChanged();
            }
        }

        protected void OpenDeletePopup()
        {
            DeletePopupIsOpen = true;
        }

        public ICommand DeleteCommand { get; protected set; }
    }
}