using System.Windows;
using System.Windows.Controls;

namespace Festispec.UI.Views.Controls
{
    public partial class DeleteCheckPopup : UserControl
    {
        public DeleteCheckPopup()
        {
            InitializeComponent();
        }

        public string Caption { get; set; }

        public string Subtext { get; set; }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            DeleteWarning.IsOpen = false;
        }
    }
}