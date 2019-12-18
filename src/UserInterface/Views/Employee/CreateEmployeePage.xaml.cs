using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
        private static readonly Regex NumericOnlyRegex = new Regex("[^0-9]+");
        
        public CreateEmployeePage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<EmployeeViewModel>();
        }

        private void NumericTextBlockOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = NumericOnlyRegex.IsMatch(e.Text);
        }

        private void Continue_OnClick(object sender, RoutedEventArgs e)
        {
            if (Password.Password == string.Empty 
                || PasswordRepeat.Password == string.Empty 
                || Password.Password != PasswordRepeat.Password)
            {
                WarningPopupLabel.Content = "Er is geen wachtwoord ingevuld of de wachtwoorden komen niet overeen.";
                WarningPopup.IsOpen = true;
                return;
            }
            
            if (Password.Password.Length < 5)
            {
                WarningPopupLabel.Content = "Het ingevoerde wachtwoord is te kort.";
                WarningPopup.IsOpen = true;
                return;
            }

            if (Username.Text.Length < 5)
            {
                WarningPopupLabel.Content = "De ingevoerde gebruikersnaam is te kort.";
                WarningPopup.IsOpen = true;
                return;
            }

            ((EmployeeViewModel) DataContext).SaveCommand.Execute(Password.Password);
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            WarningPopup.IsOpen = false;
        }
    }
}
