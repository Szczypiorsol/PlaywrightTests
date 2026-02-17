using Microsoft.Playwright;
using Serilog;
using Tests.Infrastructure;
using Tests.SwagLabs.Controls;
using static Tests.SwagLabs.Controls.Control;

namespace Tests.SwagLabs.Pages
{
    public class CheckoutOverviewPage : BasePage
    {
        private readonly ListControl _productsItemList;
        private readonly TextBox _paymentInformationTextBox;
        private readonly TextBox _shippingInformationTextBox;
        private readonly TextBox _summarySubtotalTextBox;
        private readonly TextBox _summaryTaxTextBox;
        private readonly TextBox _summaryTotalTextBox;
        private readonly Button _cancelButton;
        private readonly Button _finishButton;

        public ListControl ProductsItemList => _productsItemList;
        public TextBox PaymentInformationTextBox => _paymentInformationTextBox;
        public TextBox ShippingInformationTextBox => _shippingInformationTextBox;
        public TextBox SummarySubtotalTextBox => _summarySubtotalTextBox;
        public TextBox SummaryTaxTextBox => _summaryTaxTextBox;
        public TextBox SummaryTotalTextBox => _summaryTotalTextBox;
        public Button CancelButton => _cancelButton;
        public Button FinishButton => _finishButton;

        public CheckoutOverviewPage(IPage page) : base(page, "CheckoutOverviewPage")
        {
            _productsItemList = new ListControl(_page, GetBy.CssSelector, "div.cart_list",GetBy.CssSelector, "div.cart_item");
            _paymentInformationTextBox = new TextBox(_page, GetBy.TestId, "payment-info-value");
            _shippingInformationTextBox = new TextBox(_page, GetBy.TestId, "shipping-info-value");
            _summarySubtotalTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_subtotal_label");
            _summaryTaxTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_tax_label");
            _summaryTotalTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_total_label");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel");
            _finishButton = new Button(_page, GetBy.Role, "Finish");
        }

        public override async Task InitAsync()
        {
            TestsLogger.LogInformation("Initializing [CheckoutOverviewPage]...");
            await ProductsItemList.WaitToBeVisibleAsync();
            await PaymentInformationTextBox.WaitToBeVisibleAsync();
            await ShippingInformationTextBox.WaitToBeVisibleAsync();
            await SummarySubtotalTextBox.WaitToBeVisibleAsync();
            await SummaryTaxTextBox.WaitToBeVisibleAsync();
            await SummaryTotalTextBox.WaitToBeVisibleAsync();
            await CancelButton.WaitToBeVisibleAsync();
            await FinishButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            TestsLogger.LogDebug("[CheckoutOverviewPage] initialized successfully.");
        }

        public static async Task<CheckoutOverviewPage> InitAsync(IPage page)
        {
            CheckoutOverviewPage checkoutOverviewPage = new(page);
            await checkoutOverviewPage.InitAsync();
            return checkoutOverviewPage;
        }

        public ILocator GetProductNameLocator(int index)
        {
            return _productsItemList.GetItemElementLocator(index, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetProductPriceLocator(int index)
        {
            return _productsItemList.GetItemElementLocator(index, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<CheckoutPage> ClickCancelAsync()
        {
            TestsLogger.LogInformation("Clicking [Cancel] button on [CheckoutOverviewPage]...");
            EnsureInitialized();
            await CancelButton.ClickAsync();
            TestsLogger.LogDebug("[Cancel] button clicked.");
            return await CheckoutPage.InitAsync(_page);
        }

        public async Task<CheckoutCompletePage> ClickFinishAsync()
        {
            TestsLogger.LogInformation("Clicking [Finish] button on [CheckoutOverviewPage]...");
            EnsureInitialized();
            await _finishButton.ClickAsync();
            TestsLogger.LogDebug("[Finish] button clicked.");
            return await CheckoutCompletePage.InitAsync(_page);
        }
    }
}