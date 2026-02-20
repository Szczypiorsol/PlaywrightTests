using Microsoft.Playwright;
using Tests.Infrastructure;
using Tests.SwagLabs.Controls;
using static Tests.SwagLabs.Controls.Control;

namespace Tests.SwagLabs.Pages
{
    public class CheckoutPage : BasePage
    {
        private readonly TextBox _firstNameTextBox;
        private readonly TextBox _lastNameTextBox;
        private readonly TextBox _postalCodeTextBox;
        private readonly Button _cancelButton;
        private readonly Button _continueButton;
        private readonly TextBox _errorMessageTextBox;

        public TextBox FirstNameTextBox => _firstNameTextBox;
        public TextBox LastNameTextBox => _lastNameTextBox;
        public TextBox PostalCodeTextBox => _postalCodeTextBox;
        public Button CancelButton => _cancelButton;
        public Button ContinueButton => _continueButton;
        public TextBox ErrorMessageTextBox => _errorMessageTextBox;

        public CheckoutPage(IPage page) : base(page, "CheckoutPage")
        {
            _firstNameTextBox = new TextBox(_page, GetBy.Placeholder, "First Name");
            _lastNameTextBox = new TextBox(_page, GetBy.Placeholder, "Last Name");
            _postalCodeTextBox = new TextBox(_page, GetBy.Placeholder, "Zip/Postal Code");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel");
            _continueButton = new Button(_page, GetBy.Role, "Continue");
            _errorMessageTextBox = new TextBox(_page, GetBy.CssSelector, "h3");
        }

        public override async Task InitAsync()
        {
            TestsLogger.LogInformation("Initializing [CheckoutPage]...");
            await FirstNameTextBox.WaitToBeVisibleAsync();
            await LastNameTextBox.WaitToBeVisibleAsync();
            await PostalCodeTextBox.WaitToBeVisibleAsync();
            await CancelButton.WaitToBeVisibleAsync();
            await ContinueButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            TestsLogger.LogDebug("[CheckoutPage] initialized successfully.");
        }

        public static async Task<CheckoutPage> InitAsync(IPage page)
        {
            CheckoutPage checkoutPage = new(page);
            await checkoutPage.InitAsync();
            return checkoutPage;
        }

        public async Task<CheckoutPage> FillCheckoutInformationAsync(string firstName = "", string lastName = "", string postalCode = "")
        {
            TestsLogger.LogInformation("Filling checkout information...");
            EnsureInitialized();
            if (!string.IsNullOrWhiteSpace(firstName))
                await FirstNameTextBox.EnterTextAsync(firstName);
            if (!string.IsNullOrWhiteSpace(lastName))
                await LastNameTextBox.EnterTextAsync(lastName);
            if (!string.IsNullOrWhiteSpace(postalCode))
                await PostalCodeTextBox.EnterTextAsync(postalCode);
            TestsLogger.LogDebug("Checkout information filled.");
            return await InitAsync(_page);
        }

        public async Task<CheckoutOverviewPage> ClickContinueAsync()
        {
            TestsLogger.LogInformation("Clicking Continue button...");
            EnsureInitialized();
            await ContinueButton.ClickAsync();
            TestsLogger.LogDebug("Navigated to Checkout Overview Page.");
            return await CheckoutOverviewPage.InitAsync(_page);
        }

        public async Task<CheckoutPage> ClickContinueErrorExpectedAsync()
        {
            TestsLogger.LogInformation("Clicking Continue button expecting error...");
            EnsureInitialized();
            await ContinueButton.ClickAsync();
            TestsLogger.LogDebug("Staying on Checkout Page due to expected error.");
            return await InitAsync(_page);
        }

        public async Task<CartPage> ClickCancelAsync()
        {
            TestsLogger.LogInformation("Clicking Cancel button...");
            EnsureInitialized();
            await CancelButton.ClickAsync();
            TestsLogger.LogDebug("Navigated back to Cart Page.");
            return await CartPage.InitAsync(_page);
        }
    }
}