using Festispec.DomainServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using Festispec.UI.ViewModels;

namespace Festispec.UI
{
    public class AppServices
    {
        private AppServices()
        {
            var services = new ServiceCollection();

            //  Register Viewmodels here
            services.AddTransient<MainViewModel>();
            services.AddTransient<QuestionaireViewModel>();

            // Services from DomainServices
            services.AddServices();

            ServiceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider ServiceProvider { get; }

        private static AppServices _instance;
        private static readonly object _instanceLock = new object();
        private static AppServices GetInstance()
        {
            lock (_instanceLock)
            {
                return _instance ?? (_instance = new AppServices());
            }
        }

        public static AppServices Instance => _instance ?? GetInstance();
    }
}
