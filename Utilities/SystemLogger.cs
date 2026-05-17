using Serilog;
using Serilog.Core;

namespace RigorStarter.Utilities;

public static class SystemLogger
{
    private static Logger _logger;

    static SystemLogger()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine(XdgPaths.GetCacheDir("RigorStarter"), "logs/sys.log"),
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();
    }

    public static void Info(string message) => _logger.Information(message);

    public static void Warning(string message) => _logger.Warning(message);

    public static void Error(string message, Exception? ex = null) => _logger.Error(ex, message);

    public static void Debug(string message) => _logger.Debug(message);
}
