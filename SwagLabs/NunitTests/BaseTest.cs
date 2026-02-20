using Microsoft.Playwright;
using Serilog;
using System.Diagnostics;
using Tests.Infrastructure;
using Tests.SwagLabs.Pages;

namespace Tests.SwagLabs.NunitTests
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

        protected IPlaywright? PlaywrightInstance;
        protected IBrowser? Browser;
        protected IBrowserContext? BrowserContext;
        protected IPage? PageInstance;
        protected string UserLogin;

        protected async Task<LoginPage> NavigateToLoginPageAsync(string url = "https://www.saucedemo.com/")
        {
            await PageInstance.GotoAsync(url);
            return await LoginPage.InitAsync(PageInstance);
        }

        [OneTimeSetUp]
        public async Task OneTimeSetupAsync()
        {
            TestsLogger.LogInformation("================================================================================");
            TestsLogger.LogInformation("============================== Test Suite Started ==============================");
            TestsLogger.LogInformation("================================================================================");

            PlaywrightInstance = await Playwright.CreateAsync();
            switch (TestsConfiguration.Browser.ToLower())
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
                    TestsLogger.LogWarning("Unsupported browser type specified in configuration. Defaulting to Chromium.");
                    Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });
                    break;
            }

            UserLogin = Users[TestsConfiguration.DefaultUser];

            TestsLogger.LogDebug($"OneTimeSetup completed - Browser initialized with user: {UserLogin}");
        }

        [SetUp]
        public async Task Setup()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            TestsLogger.LogInformation("================================================================================");
            TestsLogger.LogInformation($"=== Starting test: {testName} ===");

            // Każdy test dostaje własny context i stronę (izolacja)
            BrowserContext = await Browser!.NewContextAsync();
            PageInstance = await BrowserContext.NewPageAsync();

            if (Debugger.IsAttached)
            {
                await PageInstance.PauseAsync();
            }

            TestsLogger.LogDebug($"Test context and page initialized for {testName}");
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;

            if (testResult == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                TestsLogger.LogError("Test {TestName} FAILED", testName);

                // Opcjonalnie: zrób screenshot
                if (PageInstance != null)
                {
                    var screenshotPath = $"{TestsConfiguration.ScreenshotPath}/{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
                    await PageInstance.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                    TestsLogger.LogInformation("Screenshot saved to {ScreenshotPath}", screenshotPath);
                }
            }
            else
            {
                TestsLogger.LogInformation("Test {TestName} finished with status: {TestResult}", testName, testResult);
            }
            TestsLogger.LogInformation("================================================================================");

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
            TestsLogger.LogInformation("================================================================================");
            TestsLogger.LogInformation("============================= Test Suite Completed =============================");
            TestsLogger.LogInformation("================================================================================");

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
