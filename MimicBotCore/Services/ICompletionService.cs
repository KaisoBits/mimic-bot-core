namespace MimicBotCore.Services;

public interface ICompletionService
{
    Task<string> GetCompletionAsync(IEnumerable<string> messages);
}
