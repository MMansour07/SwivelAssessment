using NLog;
using NLog.Config;
using NLog.Targets;

namespace Swivel.Infrastructure.Logging
{
    public class LoggingConfig
    {
        public static string ConnectionString
        {
            get
            {
                return AppSettings.CONNECTION_STRING;
            }
        }

        public static void LogToFile()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget { FileName = AppSettings.APP_LOGS_FILE_PATH };
            config.AddTarget("logfile", fileTarget);
            LoggingRule rule = new LoggingRule("*", LogLevel.Warn, fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
        }
    }
}
