using System.Windows.Controls;
using Festispec.UI.ViewModels.Customers;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Customer
{
    /// <summary>
    /// Interaction logic for CustomerScreen.xaml
    /// </summary>
    public partial class CustomerListPage : Page
    {
        public CustomerListPage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<CustomerListViewModel>();
        }
    }
}
