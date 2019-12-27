using Festispec.DomainServices;
using Microsoft.Extensions.DependencyInjection;
using Festispec.UI.ViewModels;
using Festispec.UI.Services;
using Festispec.UI.Interfaces;
using System;
using Festispec.UI.ViewModels.Customers;

namespace Festispec.UI
{
    public class AppServices
    {
        private AppServices()
        {
            var services = new ServiceCollection();

            //  Register Viewmodels here
            services.AddTransient<GoogleTestViewModel>();
            services.AddSingleton<MainViewModel>();

            #region Festival ViewModels
            services.AddTransient<FestivalViewModel>();
            services.AddTransient<CreateFestivalViewModel>();
            services.AddTransient<UpdateFestivalViewModel>();
            services.AddTransient<FestivalListViewModel>();
            #endregion

            #region Questionnaire ViewModels
            services.AddTransient<QuestionnaireViewModel>();
            #endregion

            #region Customer ViewModels
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<CustomerListViewModel>();
            #endregion


            services.AddTransient<RapportPreviewViewModel>();

            // Services from UI project
            services.AddSingleton<IFrameNavigationService>(RegisterRoutes());

            // Services from DomainServices
            services.AddDomainServices();

            ServiceProvider = services.BuildServiceProvider();
        }

        private static FrameNavigationService RegisterRoutes()
        {
            var navigationService = new FrameNavigationService();

            // Register your routes here
            navigationService.Configure("GoogleTest", new Uri("../Views/GoogleTestPage.xaml", UriKind.Relative));


            #region Festival Routes
            navigationService.Configure("FestivalInfo", new Uri("../Views/Festival/FestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("CreateFestival", new Uri("../Views/Festival/CreateFestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateFestival", new Uri("../Views/Festival/UpdateFestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("FestivalList", new Uri("../Views/Festival/FestivalListPage.xaml", UriKind.Relative));
            #endregion

            #region Questionnaire Routes
            navigationService.Configure("Questionnaire", new Uri("../Views/Questionnaire/QuestionnairePage.xaml", UriKind.Relative));
            #endregion

            #region Customer Routes
            navigationService.Configure("CustomerList", new Uri("../Views/Customer/CustomerListPage.xaml", UriKind.Relative));
            navigationService.Configure("CreateCustomer", new Uri("../Views/Customer/CreateCustomerPage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateCustomer", new Uri("../Views/Customer/UpdateCustomerPage.xaml", UriKind.Relative));
            navigationService.Configure("CustomerInfo", new Uri("../Views/Customer/CustomerPage.xaml", UriKind.Relative));
            #endregion

            #region Login Routes
            navigationService.Configure("LoginPageEmployee", new Uri("../Views/Login/LoginPageEmployee.xaml", UriKind.Relative));
            #endregion

            #region Home Routes
            navigationService.Configure("HomePage", new Uri("../Views/Home/HomePage.xaml", UriKind.Relative));
            #endregion

            navigationService.Configure("GenerateReport", new Uri("../Views/RapportenPreviewPage.xaml", UriKind.Relative));

            return navigationService;
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
