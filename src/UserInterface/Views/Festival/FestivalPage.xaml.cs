using System.Windows;
using Festispec.UI.ViewModels.Festivals;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Festival
{
    public partial class FestivalPage
    {
        public FestivalPage()
        {
            InitializeComponent();

            IServiceScope scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => scope.Dispose();

            DataContext = scope.ServiceProvider.GetRequiredService<FestivalViewModel>();
        }

        private void ToggleNewQuestionnairePopUp(object sender, RoutedEventArgs e)
        {
            CreateQuestionnairePopUp.IsOpen = !CreateQuestionnairePopUp.IsOpen;
        }

        private void ToggleDeleteQuestionnairePopUp(object sender, RoutedEventArgs e)
        {
            DeleteQuestionnairePopUp.IsOpen = !DeleteQuestionnairePopUp.IsOpen;
        }
    }
}