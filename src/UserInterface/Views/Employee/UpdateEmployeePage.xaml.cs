using System.Windows;
using Festispec.UI.ViewModels.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Employee
{
    public partial class UpdateEmployeePage
    {
        public UpdateEmployeePage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<EmployeeViewModel>();
        }

        private void OpenDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (!DeleteWarning.IsOpen)
                DeleteWarning.IsOpen = true;
        }

        private void CloseDeleteCheckPopUp(object sender, RoutedEventArgs e)
        {
            if (DeleteWarning.IsOpen)
                DeleteWarning.IsOpen = false;
        }
    }
}
