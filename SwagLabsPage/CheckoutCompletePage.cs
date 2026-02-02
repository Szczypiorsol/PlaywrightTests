using Controls;
using Microsoft.Playwright;
using static Controls.Control;

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
            await ThankYouMessageTextBox.WaitToBeVisibleAsync();
            await BackHomeButton.WaitToBeVisibleAsync();

            _isInitialized = true;
        }

        public static async Task<CheckoutCompletePage> InitAsync(IPage page)
        {
            CheckoutCompletePage checkoutCompletePage = new(page);
            await checkoutCompletePage.InitAsync();
            return checkoutCompletePage;
        }

        public async Task<ProductsPage> ClickBackHomeAsync()
        {
            EnsureInitialized();
            await BackHomeButton.ClickAsync();
            return await ProductsPage.InitAsync(_page);
        }
    }
}