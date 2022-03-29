using Discord;
using Microsoft.Extensions.Logging;

namespace MimicBotCore.Helpers;

public class DiscordHelpers
{
    public static LogLevel ToLoggerLogLevel(LogSeverity logSeverity)
    {
        return logSeverity switch
        {
            LogSeverity.Debug => LogLevel.Debug,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Verbose => LogLevel.Trace,
            _ => LogLevel.Error
        };
    }

    public static LogSeverity ToDiscordLogSeverity(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Debug => LogSeverity.Debug,
            LogLevel.Information => LogSeverity.Info,
            LogLevel.Warning => LogSeverity.Warning,
            LogLevel.Error => LogSeverity.Error,
            LogLevel.Critical => LogSeverity.Critical,
            LogLevel.Trace => LogSeverity.Verbose,
            _ => LogSeverity.Error
        };
    }
}