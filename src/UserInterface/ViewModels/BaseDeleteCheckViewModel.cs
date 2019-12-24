using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace Festispec.UI.ViewModels
{
    public abstract class BaseDeleteCheckViewModel : BaseValidationViewModel
    {
        private bool _deletePopupIsOpen;

        public bool DeletePopupIsOpen
        {
            get => _deletePopupIsOpen;
            set { _deletePopupIsOpen = value; RaisePropertyChanged(); }
        }

        public ICommand DeleteCommand { get; protected set; }
    }
}