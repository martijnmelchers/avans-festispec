using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using Festispec.UI.ViewModels.Customers;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Customer
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CreateCustomerPage : Page
    {
        private static readonly Regex NumericOnlyRegex = new Regex("[^0-9]+");
        
        public CreateCustomerPage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<CustomerViewModel>();
        }

        private void NumericTextBlockOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = NumericOnlyRegex.IsMatch(e.Text);
        }
    }
}
