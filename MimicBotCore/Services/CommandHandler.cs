using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MimicBotCore.Discord;
using MimicBotCore.Helpers;
using System.Reflection;

namespace MimicBotCore.Services;

public class CommandHandler : ICommandHandler
{
    private readonly ILogger<CommandHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    public CommandHandler(ILogger<CommandHandler> logger, IServiceProvider serviceProvider, DiscordSocketClient client, CommandService commands)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = client;
        _commands = commands;

        client.Log += msg => ProxyLogger("Client", msg);
        commands.Log += msg => ProxyLogger("Commands", msg);
    }

    public async Task InstallCommandsAsync()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        _logger.LogInformation("Registered Discord commands");

        _client.MessageReceived += HandleCommandAsync;
        _commands.CommandExecuted += CommandExecuted;
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        if (messageParam is not SocketUserMessage message)
            return;

        int argPos = 0;
        if (!message.HasCharPrefix('!', ref argPos) || message.Author.IsBot)
            return;

        var serviceScope = _serviceProvider.CreateScope();
        var context = new ScopedSocketCommandContext(serviceScope, _client, message);

        await _commands.ExecuteAsync(context, argPos, serviceScope.ServiceProvider);
    }

    private Task CommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
    {
        if (context is ScopedSocketCommandContext scoped)
        {
            scoped.ServiceScope.Dispose();
        }

        return Task.CompletedTask;
    }

    private Task ProxyLogger(string source, LogMessage msg)
    {
        _logger.Log(DiscordHelpers.ToLoggerLogSeverity(msg.Severity), msg.Exception, "[Discord - {source}] {message}", source, msg.Message);
        return Task.CompletedTask;
    }
}
