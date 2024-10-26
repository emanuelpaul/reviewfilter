using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReviewFilter.ThirdParty.MachineLearning
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMachineLearning(this IServiceCollection services, string filePath)
        {
            services.AddScoped<IMachineLearningService>(sp=> new MachineLearningService(filePath));
            return services;
        }
    }
}
