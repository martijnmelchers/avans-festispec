using System.Windows.Controls;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views
{
    /// <summary>
    ///     Interaction logic for QuestionnairePage.xaml
    /// </summary>
    public partial class QuestionnairePage : Page
    {
        private readonly IServiceScope _scope;

        public QuestionnairePage()
        {
            InitializeComponent();

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<QuestionnaireViewModel>();
        }
    }
}