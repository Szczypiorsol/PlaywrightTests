using Microsoft.Playwright;
using Serilog;
using SwagLabs.Pages;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace SwagLabs.PlaywrightTests
{
    public class BaseTest
    {
        protected static readonly Dictionary<string, string> Users = new()
        {
            ["StandardUser"] = "standard_user",
            ["LockedOutUser"] = "locked_out_user",
            ["ProblemUser"] = "problem_user",
            ["PerformanceGlitchUser"] = "performance_glitch_user",
            ["ErrorUser"] = "error_user",
            ["VisualUser"] = "visual_user",
        };

        private static IConfiguration _configuration;
        protected IPlaywright? PlaywrightInstance;
        protected IBrowser? Browser;
        protected IBrowserContext? BrowserContext;
        protected IPage? PageInstance;
        protected string UserLogin;
        protected ILogger Logger;

        protected async Task<LoginPage> NavigateToLoginPageAsync(string url = "https://www.saucedemo.com/")
        {
            await PageInstance.GotoAsync(url);
            return await LoginPage.InitAsync(PageInstance, Logger);
        }

        [OneTimeSetUp]
        public async Task OneTimeSetupAsync()
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
            Logger = Log.Logger;
            Logger?.Information("================================================================================");
            Logger.Information("============================== Test Suite Started ==============================");
            Logger?.Information("================================================================================");

            PlaywrightInstance = await Playwright.CreateAsync();
            switch (_configuration["Browser:Type"]?.ToLower())
            {
                case "chromium":
                    Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });
                    break;
                case "firefox":
                    Browser = await PlaywrightInstance.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });
                    break;
                case "webkit":
                    Browser = await PlaywrightInstance.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });
                    break;
                default:
                    Logger.Warning("Unsupported browser type specified in configuration. Defaulting to Chromium.");
                    Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });
                    break;
            }

            UserLogin = Users[_configuration["Browser:DefaultUser"] ?? "StandardUser"];

            Logger.Debug($"OneTimeSetup completed - Browser initialized with user: {UserLogin}");
        }

        [SetUp]
        public async Task Setup()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            Logger?.Information("================================================================================");
            Logger?.Information($"=== Starting test: {testName} ===");

            // Każdy test dostaje własny context i stronę (izolacja)
            BrowserContext = await Browser!.NewContextAsync();
            PageInstance = await BrowserContext.NewPageAsync();

            if (Debugger.IsAttached)
            {
                await PageInstance.PauseAsync();
            }

            Logger?.Debug($"Test context and page initialized for {testName}");
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;

            if (testResult == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Logger?.Error("Test {TestName} FAILED", testName);

                // Opcjonalnie: zrób screenshot
                if (PageInstance != null)
                {
                    var screenshotPath = $"logs/Screenshots/{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
                    await PageInstance.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                    Logger?.Information("Screenshot saved to {ScreenshotPath}", screenshotPath);
                }
            }
            else
            {
                Logger?.Information("Test {TestName} finished with status: {TestResult}", testName, testResult);
            }
            Logger?.Information("================================================================================");

            if (BrowserContext != null)
            {
                await BrowserContext.CloseAsync();
                BrowserContext = null;
                PageInstance = null;
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            Logger?.Information("================================================================================");
            Logger?.Information("============================= Test Suite Completed =============================");
            Logger?.Information("================================================================================");

            if (Browser != null)
            {
                await Browser.CloseAsync();
                Browser = null;
            }

            PlaywrightInstance?.Dispose();
            PlaywrightInstance = null;

            Log.CloseAndFlush();
        }
    }
}
