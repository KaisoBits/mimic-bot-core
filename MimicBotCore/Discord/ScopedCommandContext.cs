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

    ~ScopedSocketCommandContext() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        ServiceScope.Dispose();

        _disposed = true;
    }
}
