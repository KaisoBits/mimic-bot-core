using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MimicBot;
using MimicBot.Services;

namespace MimicBotCore.DependencyInjection
{
    public static class DiscordServicesExtensions
    {
        public static IServiceCollection AddDiscord(this IServiceCollection services)
        {
            services.AddSingleton(_ => new DiscordSocketClient());

            services.AddSingleton((_) => new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            }));

            services.AddSingleton<ICommandHandler, CommandHandler>();

            return services;
        }
    }
}
