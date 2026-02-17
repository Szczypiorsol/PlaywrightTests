using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Tests.Infrastructure
{
    public static class TestsLogger
    {
        private static bool _isInitialized = false;
        private static ILogger? _logger;

        private static void InitializeLogger()
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(TestsConfiguration.MinimumLevel)
                .WriteTo.Console()
                .WriteTo.File(
                    path: TestsConfiguration.LogFilePath,
                    rollingInterval: TestsConfiguration.RollingInterval,
                    outputTemplate: TestsConfiguration.LogOutputTemplate)
                .CreateLogger();
            _logger = Log.Logger;

            _isInitialized = true;
        }

        public static void LogVerbose(string messageTemplate, params object[] propertyValues)
        {
            if (!_isInitialized)
            {
                InitializeLogger();
            }
            _logger?.Verbose(messageTemplate, propertyValues);
        }

        public static void LogInformation(string messageTemplate, params object[] propertyValues)
        {
            if (!_isInitialized)
            {
                InitializeLogger();
            }
            _logger?.Information(messageTemplate, propertyValues);
        }

        public static void LogDebug(string messageTemplate, params object[] propertyValues)
        {
            if (!_isInitialized)
            {
                InitializeLogger();
            }
            _logger?.Debug(messageTemplate, propertyValues);
        }

        public static void LogWarning(string messageTemplate, params object[] propertyValues)
        {
            if (!_isInitialized)
            {
                InitializeLogger();
            }
            _logger?.Warning(messageTemplate, propertyValues);
        }

        public static void LogError(string messageTemplate, params object[] propertyValues)
        {
            if (!_isInitialized)
            {
                InitializeLogger();
            }
            _logger?.Error(messageTemplate, propertyValues);
        }

        public static void LogFatal(string messageTemplate, params object[] propertyValues)
        {
            if (!_isInitialized)
            {
                InitializeLogger();
            }
            _logger?.Fatal(messageTemplate, propertyValues);
        }
    }
}
