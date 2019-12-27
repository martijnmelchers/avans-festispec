using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

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

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).Login(Username.Text, Password.Password);
        }
    }
}
