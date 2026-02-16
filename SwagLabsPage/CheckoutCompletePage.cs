using Controls;
using Microsoft.Playwright;
using static Controls.Control;
using Serilog;

namespace SwagLabs.Pages
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
            _logger?.Information("Initializing [CheckoutCompletePage]...");
            await ThankYouMessageTextBox.WaitToBeVisibleAsync();
            await BackHomeButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            _logger.Debug("[CheckoutCompletePage] initialized successfully.");
        }

        public static async Task<CheckoutCompletePage> InitAsync(IPage page)
        {
            CheckoutCompletePage checkoutCompletePage = new(page);
            await checkoutCompletePage.InitAsync();
            return checkoutCompletePage;
        }

        public async Task<ProductsPage> ClickBackHomeAsync()
        {
            _logger.Information("Clicking [Back Home] button on [CheckoutCompletePage]...");
            EnsureInitialized();
            await BackHomeButton.ClickAsync();
            _logger.Debug("Clicked [Back Home] button on [CheckoutCompletePage]. Navigating to [ProductsPage]...");
            return await ProductsPage.InitAsync(_page);
        }
    }
}