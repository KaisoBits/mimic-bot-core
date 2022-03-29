using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using MimicBotCore.Discord;

namespace MimicBotCore.CommandsModules;

public class MimicModule : ModuleBase<ScopedSocketCommandContext>
{
    private readonly ILogger<MimicModule> _logger;

    public MimicModule(ILogger<MimicModule> logger)
    {
        _logger = logger;
    }

    [Command("test")]
    public async Task Mimic(string user = "self")
    {
        _logger.LogInformation("User '{username}' issued the command - '{command}' with '{message}'", 
            Context.Message.Author.Username, nameof(Mimic), Context.Message.Content);

        if(Emote.TryParse("👍", out Emote result))
        {
            await Context.Message.AddReactionAsync(result);
        }
    }
}
