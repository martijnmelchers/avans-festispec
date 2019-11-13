using Festispec.DomainServices.Interfaces;
using Festispec.UI.Views;
using System.Windows;

namespace Festispec.UI.ViewModels
{
    public class MainViewModel
    {
        public string Name { get => "John Doe";  }

        private readonly IExampleService _exampleService;
        public MainViewModel(IExampleService exampleService)
        {
            _exampleService = exampleService;
            new QuestionaireViewModel(new Models.Questionnaire());
        }

        public void openQuestionairePage()
        {
            new QuestionairePage();
        }
    }
}
