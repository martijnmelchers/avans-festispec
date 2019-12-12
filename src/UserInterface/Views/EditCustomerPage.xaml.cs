using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for EditCustomerPage.xaml
    /// </summary>
    public partial class EditCustomerPage : Page
    {
        private static readonly Regex _numericOnlyRegex = new Regex("[^0-9]+");
        
        public EditCustomerPage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<CustomerViewModel>();
        }

        private void NumericTextBlockOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _numericOnlyRegex.IsMatch(e.Text);
        }

        private void OpenDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (!DeleteWarning.IsOpen)
                DeleteWarning.IsOpen = true;
        }

        private void StopDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (DeleteWarning.IsOpen)
                DeleteWarning.IsOpen = false;
        }

        private void ContinueDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (DeleteWarning.IsOpen)
                DeleteWarning.IsOpen = false;
        }
    }
}
