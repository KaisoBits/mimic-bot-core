using OpenAI_API;

namespace MimicBotCore.Services;

public class OpenAiService : ICompletionService
{
    private static readonly string _templateMimick = File.ReadAllText("template.txt");

    private readonly OpenAIAPI _api;

    public OpenAiService(OpenAIAPI api)
    {
        _api = api;
    }

    public async Task<string> GetCompletionAsync(IEnumerable<string> messages)
    {
        string? message;
        int retries = 0;
        do
        {
            message = await InternalGetCompletionAsync(messages);
            retries++;
        } while (string.IsNullOrWhiteSpace(message) && retries < 2);

        return string.IsNullOrWhiteSpace(message) ? "[Mimic]: Try it again..." : message;
    }

    private async Task<string?> InternalGetCompletionAsync(IEnumerable<string> messages)
    {
        var result = await _api.Completions.CreateCompletionAsync(GetFilledTemplate(messages),
               temperature: 0.86,
               max_tokens: 150,
               presencePenalty: 0.65,
               stopSequences: new[] { "\n" }
           );

        return result.Completions.FirstOrDefault()?.Text;
    }

    private static string GetFilledTemplate(IEnumerable<string> messages)
    {
        var mStr = string.Join('\n', messages.Select(
            m => '-' + m.Replace('\n', ' ')
        ));
        return _templateMimick.Replace("@m", mStr);
    }
}
