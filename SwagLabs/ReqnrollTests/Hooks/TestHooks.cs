using Microsoft.Playwright;
using Reqnroll;
using Serilog;
using System.Diagnostics;
using Tests.Infrastructure;

namespace Tests.SwagLabs.ReqnrollTests.Hooks
{
    [Binding]
    internal class TestHooks
    {
        private static IPlaywright? PlaywrightInstance;
        private static IBrowser? Browser;
        private static IBrowserContext? BrowserContext;

        [BeforeTestRun]
        public static async Task BeforeTestRun()
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
            TestsLogger.LogDebug($"BeforeTestRun completed - Browser initialized.");
        }

        [BeforeScenario]
        public static async Task BeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Before Scenario");
            string testName = TestContext.CurrentContext.Test.Name;
            TestsLogger.LogInformation("================================================================================");
            TestsLogger.LogInformation($"=== Starting test: {testName} ===");

            // Każdy test dostaje własny context i stronę (izolacja)
            BrowserContext = await Browser!.NewContextAsync();
            IPage PageInstance = await BrowserContext.NewPageAsync();

            if (Debugger.IsAttached)
            {
                await PageInstance.PauseAsync();
            }

            scenarioContext.Set(BrowserContext);
            scenarioContext.Set(PageInstance);

            TestsLogger.LogDebug($"Test context and page initialized for {testName}");
        }

        [AfterScenario]
        public static async Task AfterScenario(ScenarioContext scenarioContext)
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;
            IPage? PageInstance = null;

            if (scenarioContext.ContainsKey("PageInstance"))
            {
                PageInstance = scenarioContext.Get<IPage>("PageInstance");
            }

            if (scenarioContext.ContainsKey("BrowserContext"))
            {
                BrowserContext = scenarioContext.Get<IBrowserContext>("BrowserContext");
            }

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
            }
        }

        [AfterTestRun]
        public static async Task AfterTestRun() 
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
