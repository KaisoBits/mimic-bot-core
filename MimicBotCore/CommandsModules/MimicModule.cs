using System.Text;
using System.Text.RegularExpressions;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using MimicBotCore.Discord;
using MimicBotCore.Services;

namespace MimicBotCore.CommandsModules;

public class MimicModule : ModuleBase<ScopedSocketCommandContext>
{
    private readonly static IReadOnlyDictionary<string, Emoji> _emojis = new Dictionary<string, Emoji>
    {
        { "success", new Emoji("👍") },
        { "fail", new Emoji("👎") },
        { "openAiFail", new Emoji("🤖") },
        { "discordFail", new Emoji("☎") },
    };
    private readonly static Regex _regex = new("^[a-zA-Z].{0,60}$", RegexOptions.Compiled);

    private readonly ILogger<MimicModule> _logger;
    private readonly ICompletionService _completionService;

    public MimicModule(ILogger<MimicModule> logger, ICompletionService completionService)
    {
        _logger = logger;
        _completionService = completionService;
    }

    [Command("mimic")]
    public async Task Mimic(string user = "self", int limit = 2000, int sample = 6)
    {
        _logger.LogInformation("User '{username}' issued the command '{command}' with '{message}'",
            $"{Context.Message.Author.Username}#{Context.Message.Author.Discriminator}", nameof(Mimic), Context.Message.Content);

        ulong userId;
        if (user == "self")
        {
            userId = Context.Message.Author.Id;
        }
        else
        {
            if (!MentionUtils.TryParseUser(user, out userId))
            {
                await Context.Message.AddReactionAsync(_emojis["fail"]);
                await ReplyAsync("Invalid user");
                return;
            }
        }

        await Context.Message.AddReactionAsync(_emojis["success"]);

        List<string> messages;
        try
        {
            messages = (await Context.Message.Channel
                .GetMessagesAsync(limit)
                .FlattenAsync())
                .Where(m => _regex.IsMatch(m.Content) && m.Author.Id == userId)
                .OrderBy(m => Random.Shared.Next())
                .Select(m => m.Content)
                .Take(sample)
                .ToList();
        }
        catch (Exception)
        {
            await Context.Message.AddReactionAsync(_emojis["discordFail"]);
            throw;
        }

        StringBuilder sb = new();
        sb.AppendLine("Samples:");
        foreach (var m in messages)
            sb.AppendLine("- " + m);

        _logger.LogInformation("{samples}", sb);

        string message = "";
        try
        {
            message = await _completionService.GetCompletionAsync(messages);
        }
        catch (Exception)
        {
            await Context.Message.AddReactionAsync(_emojis["openAiFail"]);
            throw;
        }

        _logger.LogInformation("Result: {message}", message);

        await ReplyAsync(message);
    }
}
