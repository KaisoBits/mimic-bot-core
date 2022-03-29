using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace MimicBotCore.Discord;

public class ScopedSocketCommandContext : SocketCommandContext, IDisposable
{
    public IServiceScope ServiceScope { get; }

    private bool _disposed = false;

    public ScopedSocketCommandContext(IServiceScope serviceScope, DiscordSocketClient client, SocketUserMessage msg) 
        : base(client, msg)
    {
        ServiceScope = serviceScope;
    }

    ~ScopedSocketCommandContext()
    {
        Dispose();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        ServiceScope.Dispose();
    }
}
