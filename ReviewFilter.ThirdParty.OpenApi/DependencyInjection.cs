using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using ReviewFilter.ThirdParty.OpenApi.Clients;
using ReviewFilter.ThirdParty.OpenApi.Engines;

namespace ReviewFilter.ThirdParty.OpenApi;

public static class DependencyInjection
{
    public static void AddOpenApi(this IServiceCollection services, string apiKey)
    {
        services.AddHttpClient<OpenAIModerationClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://api.openai.com");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        });

        services.AddScoped<IVerificationContentEngine, VerificationContentEngine>();
    }
}