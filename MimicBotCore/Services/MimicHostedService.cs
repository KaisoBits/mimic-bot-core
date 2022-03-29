using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MimicBotCore.Services;

public class MimicHostedService : IHostedService
{
    private readonly ILogger<MimicHostedService> _logger;
    private readonly IConfiguration _configuration;
    private readonly DiscordSocketClient _client;
    private readonly ICommandHandler _commandHandler;

    public MimicHostedService(ILogger<MimicHostedService> logger, IConfiguration configuration,
        DiscordSocketClient client, ICommandHandler commandHandler)
    {
        _logger = logger;
        _configuration = configuration;
        _client = client;
        _commandHandler = commandHandler;
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        await _commandHandler.InstallCommandsAsync();

        await _client.LoginAsync(TokenType.Bot, _configuration["discordApiKey"]);
        await _client.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if(_client.LoginState == LoginState.LoggedIn)
        {
            await _client.LogoutAsync();
        }
    }
}

