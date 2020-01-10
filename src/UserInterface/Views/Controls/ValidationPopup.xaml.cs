using System.Windows;

namespace Festispec.UI.Views.Controls
{
    public partial class ValidationPopup
    {
        public ValidationPopup()
        {
            InitializeComponent();
        }

        public string Caption { get; set; }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            WarningPopup.IsOpen = false;
        }
    }
}