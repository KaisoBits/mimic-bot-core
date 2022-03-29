using OpenAI_API;

namespace MimicBotCore.Services;

public class OpenAiService : ICompletionService
{
    const string _templateMimick = @"Mimick messages in Polish by generating a new, coherent message based on user's previous messages

Previous:
-nie mogę się doczekać aż zagram tą nową postacią w lidze
-czekam na was na kanale
-ja pierdole xD
-kot mi łazi po klawiaturze
-pomocy!

New: kot mi pierdoli po klawiaturze jak staram się zagrać nową postacią u was na kanale XD pomocy, czekam na kanale! 

Previous:
-nie masz psychy żeby tu przyjść
-nie piję kawy
-jesteś pojebany
-moj ojciec to alkoholik
-nie moge z ciebie

New: Nie pije kawy, bo mój ojciec to alkoholik. No po prostu nie moge z ciebie bo totalnie nie masz psychy żeby tu przyjść, bo jesteś pojebany.

Previous:
@m

New:";

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
