using Festispec.Models.Questions;
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
using Festispec.Models.Questions;
using System.Globalization;

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for QuestionnairePage.xaml
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
