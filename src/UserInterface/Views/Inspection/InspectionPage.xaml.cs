using System.Windows.Controls;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views
{
    /// <summary>
    ///     Interaction logic for InspectionPage.xaml
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