using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimicBotCore.Services;
using OpenAI_API;

namespace MimicBotCore.DependencyInjection;

public static class AiCompletionServicesExtensions
{
    public static IServiceCollection AddAiCompletions(this IServiceCollection services)
    {
        services.AddSingleton(s =>
        {
            var config = s.GetRequiredService<IConfiguration>();
            return new OpenAIAPI(config["openAiApiKey"], new Engine(config["OpenAi:Engine"]));
        });

        services.AddTransient<ICompletionService, OpenAiService>();

        return services;
    }
}
