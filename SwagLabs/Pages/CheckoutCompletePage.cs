using Microsoft.Playwright;
using Serilog;
using Tests.Infrastructure;
using Tests.SwagLabs.Controls;
using static Tests.SwagLabs.Controls.Control;

namespace Tests.SwagLabs.Pages
{
    public class CheckoutCompletePage : BasePage
    {
        private readonly TextBox _thankYouMessageTextBox;
        private readonly Button _backHomeButton;

        public TextBox ThankYouMessageTextBox => _thankYouMessageTextBox;
        public Button BackHomeButton => _backHomeButton;

        public CheckoutCompletePage(IPage page) : base(page, "CheckoutCompletePage")
        {
            _thankYouMessageTextBox = new TextBox(_page, GetBy.CssSelector, "h2.complete-header");
            _backHomeButton = new Button(_page, GetBy.Role, "Back Home");
        }

        public override async Task InitAsync()
        {
            TestsLogger.LogInformation("Initializing [CheckoutCompletePage]...");
            await ThankYouMessageTextBox.WaitToBeVisibleAsync();
            await BackHomeButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            TestsLogger.LogDebug("[CheckoutCompletePage] initialized successfully.");
        }

        public static async Task<CheckoutCompletePage> InitAsync(IPage page)
        {
            CheckoutCompletePage checkoutCompletePage = new(page);
            await checkoutCompletePage.InitAsync();
            return checkoutCompletePage;
        }

        public async Task<ProductsPage> ClickBackHomeAsync()
        {
            TestsLogger.LogInformation("Clicking [Back Home] button on [CheckoutCompletePage]...");
            EnsureInitialized();
            await BackHomeButton.ClickAsync();
            TestsLogger.LogDebug("Clicked [Back Home] button on [CheckoutCompletePage]. Navigating to [ProductsPage]...");
            return await ProductsPage.InitAsync(_page);
        }
    }
}