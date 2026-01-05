using Microsoft.Playwright;
using System.Diagnostics;

namespace SwagLabs
{
    public class BaseTest
    {
        protected IPlaywright? PlaywrightInstance;
        protected IBrowser? Browser;
        protected IBrowserContext? BrowserContext;
        protected IPage? PageInstance;

        [OneTimeSetUp]
        public async Task OneTimeSetupAsync()
        {
            PlaywrightInstance = await Playwright.CreateAsync();
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !Debugger.IsAttached });
        }

        [SetUp]
        public async Task Setup()
        {
            // Każdy test dostaje własny context i stronę (izolacja)
            BrowserContext = await Browser!.NewContextAsync();
            PageInstance = await BrowserContext.NewPageAsync();

            if (Debugger.IsAttached)
            {
                await PageInstance.PauseAsync();
            }
        }

        [TearDown]
        public async Task TearDownAsync()
        {
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
            if (Browser != null)
            {
                await Browser.CloseAsync();
                Browser = null;
            }

            if (PlaywrightInstance != null)
            {
                PlaywrightInstance.Dispose();
                PlaywrightInstance = null;
            }
        }
    }
}
