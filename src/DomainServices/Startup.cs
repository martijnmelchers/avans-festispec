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

            services.AddScoped<IQuestionService, QuestionService>();

            services.AddScoped<FestispecContext>();
            services.AddScoped<IInspectionService, InspectionService>();

            // Register all your factories here
            // Example: services.AddSingleton(new ExampleFactory());
            services.AddSingleton(new QuestionFactory());
            
            // Database initialisation code below
            using (var ctx = new FestispecContext()) ctx.Database.Initialize(false);

            return services;
        }
    }
}
