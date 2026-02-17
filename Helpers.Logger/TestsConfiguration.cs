using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Tests.Infrastructure
{
    public static class TestsConfiguration
    {
        private static bool _isInitialized = false;
        private static string _browser = "chromium";
        private static string _defaultUser = "StandardUser";
        private static readonly string _defaultPassword = "secret_sauce";
        private static string _logFilePath = "./log-.log";
        private static string _screenshotPath = "C:/SS/Screenshots/";
        private static RollingInterval _rollingInterval = RollingInterval.Day;
        private static LogEventLevel _minimumLevel = LogEventLevel.Information;
        private static readonly string _logOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static string Browser
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _browser;
            }
        }
        public static string DefaultUser
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _defaultUser;
            }
        }
        public static string DefaultPassword
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _defaultPassword;
            }
        }
        public static string LogFilePath
        {
            get 
            { 
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _logFilePath; 
            } 
        }
        public static string ScreenshotPath
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _screenshotPath;
            }
        }
        public static RollingInterval RollingInterval
        {             
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _rollingInterval;
            }
        }
        public static LogEventLevel MinimumLevel
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _minimumLevel;
            }
        }
        public static string LogOutputTemplate
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }
                return _logOutputTemplate;
            }
        }

        private static void Initialize()
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddEnvironmentVariables()
                //.AddUserSecrets<BaseTest>(optional: true)
                .Build();

            _browser = _configuration["Browser"] ?? _browser;
            _defaultUser = _configuration["DefaultUser"] ?? _defaultUser;
            _logFilePath = _configuration["Logger:LogFilePath"] ?? _logFilePath;
            _screenshotPath = _configuration["ScreenshotPath"] ?? _screenshotPath;
            _rollingInterval = Enum.TryParse<RollingInterval>(_configuration["Logger:RollingInterval"], true, out var rollingInterval) ? rollingInterval : _rollingInterval;
            _minimumLevel = Enum.TryParse<LogEventLevel>(_configuration["Logger:MinimumLevel"], true, out var level) ? level : _minimumLevel;

            _isInitialized = true;
        }
    }
}
