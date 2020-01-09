using Festispec.UI.ViewModels.Festivals;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Festival
{
    public partial class CreateFestivalPage
    {
        public CreateFestivalPage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<CreateFestivalViewModel>();
        }
    }
}