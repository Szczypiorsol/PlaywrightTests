using Serilog;
using Microsoft.Extensions.Configuration;

namespace Helpers
{
    public class LogHelper
    {
        private static ILogger? _logger;
        private static IConfiguration? _configuration;
        
        public static ILogger? Logger => _logger;

        public static void InitializeLogger()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddEnvironmentVariables()
                //.AddUserSecrets<BaseTest>(optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(Enum.TryParse<Serilog.Events.LogEventLevel>(_configuration["Logger:MinimumLevel"], true, out var level) ? level : Serilog.Events.LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.File(
                    path: _configuration["Logger:LogFilePath"].ToString(),
                    rollingInterval: Enum.TryParse<RollingInterval>(_configuration["Logger:RollingInterval"], true, out var rollingInterval) ? rollingInterval : RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            _logger = Log.Logger;
        }
    }
}
