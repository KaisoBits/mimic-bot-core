using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace MimicBotCore.Discord;

public class ScopedSocketCommandContext : SocketCommandContext
{
    public IServiceScope ServiceScope { get; }

    public ScopedSocketCommandContext(IServiceScope serviceScope, DiscordSocketClient client, SocketUserMessage msg) 
        : base(client, msg)
    {
        ServiceScope = serviceScope;
    }
}
