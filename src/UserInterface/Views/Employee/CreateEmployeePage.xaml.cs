using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.UI.ViewModels.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Employee
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class CreateEmployeePage : Page
    {
        
        public CreateEmployeePage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<EmployeeViewModel>();
        }

        private void Continue_OnClick(object sender, RoutedEventArgs e)
        {
            // This code-behind event needs to happen because we cannot bind on any of the PasswordBox properties.
            ((EmployeeViewModel) DataContext).SaveCommand.Execute(new PasswordWithVerification
            {
                Password = Password.SecurePassword.Copy(),
                VerificationPassword = PasswordRepeat.SecurePassword.Copy()
            });
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            WarningPopup.IsOpen = false;
        }
    }
}
