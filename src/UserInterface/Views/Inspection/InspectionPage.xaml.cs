using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for InspectionPage.xaml
    /// </summary>
    public partial class InspectionPage : Page
    {
        private readonly IServiceScope _scope;

        public InspectionPage()
        {
            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<InspectionViewModel>();
        }
    }
}
