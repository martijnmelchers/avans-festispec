using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.DomainServices
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register all your services here
            services.AddScoped<IExampleService, ExampleService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<FestispecContext>();
            services.AddScoped<IQuestionService, QuestionService>();
            return services;
        }
    }
}
