using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Festispec.UI.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginPageEmployee.xaml
    /// </summary>
    public partial class LoginPageEmployee : Page
    {
        public LoginPageEmployee()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}