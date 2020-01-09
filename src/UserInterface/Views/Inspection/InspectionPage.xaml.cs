using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Inspection
{
    public partial class InspectionPage
    {
        public InspectionPage()
        {
            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<InspectionViewModel>();
        }
    }
}