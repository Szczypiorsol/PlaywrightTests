using Controls;
using Microsoft.Playwright;
using NUnit.Framework;
using static Controls.Control;

namespace SwagLabs.Pages
{
    public class CheckoutOverviewPage : BasePage
    {
        private readonly ListControl _overviewItemList;
        private readonly TextBox _paymentInformationTextBox;
        private readonly TextBox _shippingInformationTextBox;
        private readonly TextBox _summarySubtotalTextBox;
        private readonly TextBox _summaryTaxTextBox;
        private readonly TextBox _summaryTotalTextBox;
        private readonly Button _cancelButton;
        private readonly Button _finishButton;

        public CheckoutOverviewPage(IPage page) : base(page, "CheckoutOverviewPage")
        {
            _overviewItemList = new ListControl(
                _page, 
                GetBy.CssSelector, 
                "div.cart_list",
                $"{_pageName}_[ProductsList]", 
                GetBy.CssSelector, 
                "div.cart_item",
                $"{_pageName}_[Product]"
                );
            _paymentInformationTextBox = new TextBox(_page, GetBy.TestId, "payment-info-value", $"{_pageName}_[PaymentInformationTextBox]");
            _shippingInformationTextBox = new TextBox(
                _page, 
                GetBy.TestId, 
                "shipping-info-value",
                $"{_pageName}_[ShippingInformationTextBox]"
                );
            _summarySubtotalTextBox = new TextBox(
                _page, 
                GetBy.CssSelector, 
                "div.summary_subtotal_label",
                $"{_pageName}_[SummarySubtotalTextBox]"
                );
            _summaryTaxTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_tax_label", $"{_pageName}_[SummaryTaxTextBox]");
            _summaryTotalTextBox = new TextBox(_page, GetBy.CssSelector, "div.summary_total_label", $"{_pageName}_[SummaryTotalTextBox]");
            _cancelButton = new Button(_page, GetBy.Role, "Cancel", $"{_pageName}_[CancelButton]");
            _finishButton = new Button(_page, GetBy.Role, "Finish", $"{_pageName}_[FinishButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _overviewItemList.CheckIsVisibleAsync();
                await _paymentInformationTextBox.CheckIsVisibleAsync();
                await _shippingInformationTextBox.CheckIsVisibleAsync();
                await _summarySubtotalTextBox.CheckIsVisibleAsync();
                await _summaryTaxTextBox.CheckIsVisibleAsync();
                await _summaryTotalTextBox.CheckIsVisibleAsync();
                await _cancelButton.CheckIsVisibleAsync();
                await _finishButton.CheckIsVisibleAsync();
            }
            catch (Exception ex) when (ex is AssertionException || ex is PlaywrightException)
            {
                throw new AssertionException($"{_pageName} did not load correctly.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"{_pageName} did not load within {_defaultTimeout} miliseconds.", ex);
            }

            _isInitialized = true;
        }

        public static async Task<CheckoutOverviewPage> InitAsync(IPage page)
        {
            CheckoutOverviewPage checkoutOverviewPage = new(page);
            await checkoutOverviewPage.InitAsync();
            return checkoutOverviewPage;
        }

        public ILocator GetOverviewItemListLocator()
        {
            return _overviewItemList.Locator;
        }

        public ILocator GetOverviewItemListItemsLocator()
        {
            return _overviewItemList.ListItemLocator;
        }

        public ILocator GetOverviewItemAtLocator(int index)
        {
            return _overviewItemList.GetItemLocatorByOrdinalNumber(index);
        }

        public ILocator GetOverviewItemNameAtLocator(int index)
        {
            return _overviewItemList.GetItemElementLocatorAsync(index, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetOverviewItemPriceAtLocator(int index)
        {
            return _overviewItemList.GetItemElementLocatorAsync(index, GetBy.CssSelector, "div.inventory_item_price");
        }

        public ILocator GetPaymentInformationLocator()
        {
            return _paymentInformationTextBox.Locator;
        }

        public ILocator GetShippingInformationLocator()
        {
            return _shippingInformationTextBox.Locator;
        }

        public ILocator GetSummarySubtotalLocator()
        {
            return _summarySubtotalTextBox.Locator;
        }

        public ILocator GetSummaryTaxLocator()
        {
            return _summaryTaxTextBox.Locator;
        }

        public ILocator GetSummaryTotalLocator()
        {
            return _summaryTotalTextBox.Locator;
        }

        public ILocator GetCancelButtonLocator()
        {
            return _cancelButton.Locator;
        }

        public ILocator GetFinishButtonLocator()
        {
            return _finishButton.Locator;
        }

        public async Task<int> GetOverviewItemsCountAsync()
        {
            EnsureInitialized();
            return await _overviewItemList.GetItemCountAsync();
        }

        public async Task<string> GetOverviewItemNameAtAsync(int index)
        {
            EnsureInitialized();
            return await _overviewItemList.GetItemElementTextAsync(index, GetBy.CssSelector, "div.inventory_item_name");
        }

        public async Task<string> GetOverviewItemPriceAtAsync(int index)
        {
            EnsureInitialized();
            return await _overviewItemList.GetItemElementTextAsync(index, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<string> GetPaymentInformationAsync()
        {
            EnsureInitialized();
            return await _paymentInformationTextBox.GetTextAsync();
        }

        public async Task<string> GetShippingInformationAsync()
        {
            EnsureInitialized();
            return await _shippingInformationTextBox.GetTextAsync();
        }

        public async Task<string> GetSummarySubtotalAsync()
        {
            EnsureInitialized();
            return await _summarySubtotalTextBox.GetTextAsync();
        }

        public async Task<string> GetSummaryTaxAsync()
        {
            EnsureInitialized();
            return await _summaryTaxTextBox.GetTextAsync();
        }

        public async Task<string> GetSummaryTotalAsync()
        {
            EnsureInitialized();
            return await _summaryTotalTextBox.GetTextAsync();
        }

        public async Task<CheckoutPage> ClickCancelAsync()
        {
            EnsureInitialized();
            try
            {
                await _cancelButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click cancel button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CheckoutPage.InitAsync(_page);
        }

        public async Task<CheckoutCompletePage> ClickFinishAsync()
        {
            EnsureInitialized();
            try
            {
                await _finishButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click finish button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CheckoutCompletePage.InitAsync(_page);
        }
    }
}