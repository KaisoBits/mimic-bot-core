using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimicBotCore.Services;

namespace MimicBotCore.DependencyInjection;

public static class DiscordServicesExtensions
{
    public static IServiceCollection AddDiscord(this IServiceCollection services)
    {
        services.Configure<CommandServiceConfig>(cfg =>
        {
            cfg.CaseSensitiveCommands = false;
            cfg.DefaultRunMode = RunMode.Async;
            cfg.LogLevel = LogSeverity.Verbose;
        });

        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton(s => new CommandService(s.GetRequiredService<IOptions<CommandServiceConfig>>().Value));

        services.AddSingleton<ICommandHandler, CommandHandler>();

        return services;
    }
}
