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
            // Register all your services here
            services.AddScoped<IExampleService, ExampleService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IQuestionnaireService, QuestionnaireService>();
            services.AddScoped<IFestivalService, FestivalService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IQuestionService, QuestionService>();

            // TODO: Find a way to make it scoped again. Probably won't matter but should be possible.
            services.AddTransient<FestispecContext>();
            services.AddScoped(typeof(SyncService<>));

            // Register all your factories here
            // Example: services.AddSingleton(new ExampleFactory());
            services.AddSingleton(new QuestionFactory());
            services.AddSingleton<OfflineService>();
            
            // Database initialisation code below
            using (var ctx = new FestispecContext()) ctx.Database.Initialize(false);

            return services;
        }
    }
}
