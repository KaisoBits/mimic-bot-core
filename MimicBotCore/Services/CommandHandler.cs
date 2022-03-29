using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace MimicBot.Services;

public class CommandHandler : ICommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        _client = client;
        _commands = commands;
    }

    public async Task InstallCommandsAsync()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

        _client.MessageReceived += HandleCommandAsync;
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        if (messageParam is not SocketUserMessage message)
            return;

        int argPos = 0;
        if (!message.HasCharPrefix('!', ref argPos) || message.Author.IsBot)
            return;

        var context = new SocketCommandContext(_client, message);

        await _commands.ExecuteAsync(context, argPos, null);
    }
}
