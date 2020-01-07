using Festispec.DomainServices.Enums;
using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models.EntityMapping;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.DomainServices
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<FestispecContext>();
            services.AddScoped(typeof(ISyncService<>), typeof(JsonSyncService<>));
            services.AddSingleton<IOfflineService, DbPollOfflineService>();
            
            // Register services for *both* online and offline here
            services.AddScoped<IExampleService, ExampleService>();
            
            // Register all your online services here
            if (services.BuildServiceProvider().GetRequiredService<DbPollOfflineService>().IsOnline)
            {
                services.AddScoped<IAuthenticationService, AuthenticationService>();
                services.AddScoped<IQuestionnaireService, QuestionnaireService>();
                services.AddScoped<IFestivalService, FestivalService>();
                services.AddScoped<ICustomerService, CustomerService>();
                services.AddScoped<IEmployeeService, EmployeeService>();
                services.AddScoped<IQuestionService, QuestionService>();
                services.AddScoped<IInspectionService, InspectionService>();
                
                // Database initialisation code below
                using (var ctx = new FestispecContext()) ctx.Database.Initialize(false);
            }
            else
            {
                services.AddScoped<IAuthenticationService, OfflineAuthenticationService>();
                services.AddScoped<IQuestionnaireService, OfflineQuestionnaireService>();
                services.AddScoped<IFestivalService, OfflineFestivalService>();
                services.AddScoped<ICustomerService, OfflineCustomerService>();
                services.AddScoped<IEmployeeService, OfflineEmployeeService>();
                services.AddScoped<IQuestionService, QuestionService>(); // TODO
                services.AddScoped<IInspectionService, InspectionService>(); // TODO
            }

            // Register all your factories here
            // Example: services.AddSingleton(new ExampleFactory());
            services.AddSingleton(new QuestionFactory());

            return services;
        }
    }
}
