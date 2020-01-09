using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views
{
    public partial class RapportPreviewPage
    {
        public RapportPreviewPage()
        {
            InitializeComponent();


            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<RapportPreviewViewModel>();
        }
    }
}