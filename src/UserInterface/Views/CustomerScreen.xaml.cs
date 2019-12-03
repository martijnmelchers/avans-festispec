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

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for CustomerScreen.xaml
    /// </summary>
    public partial class CustomerScreen : Page
    {
        public CustomerScreen()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<CustomerListViewModel>();
        }

        private void OpenDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (!DeleteWarning.IsOpen)
            {
                DeleteWarning.IsOpen = true;
            }
        }

        private void StopDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (DeleteWarning.IsOpen)
            {
                DeleteWarning.IsOpen = false;
            }
        }

        private void ContinueDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (DeleteWarning.IsOpen)
            {
                DeleteWarning.IsOpen = false;
            }
        }
    }
}
