using System.Windows;
using System.Windows.Controls;

namespace Festispec.UI.Views.Controls
{
    public partial class ValidationPopup : UserControl
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