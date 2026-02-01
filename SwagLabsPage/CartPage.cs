using Controls;
using Microsoft.Playwright;
using NUnit.Framework;
using static Controls.Control;

namespace SwagLabs.Pages
{
    public class CartPage : BasePage
    {
        private readonly ListControl _cartList;
        private readonly Button _continueShoppingButton;
        private readonly Button _checkoutButton;

        public CartPage(IPage page) : base(page, "CartPage")
        {
            _cartList = new ListControl(
                page: _page,
                getByList: GetBy.CssSelector,
                listName: "div.cart_list",
                listDescription: $"{_pageName}_[ProductsList]",
                getByItem: GetBy.CssSelector,
                itemName: "div.cart_item",
                listItemDescription: $"{_pageName}_[Product]"
                );
            _continueShoppingButton = new Button(_page, GetBy.Role, "Continue Shopping", $"{_pageName}_[ContinueShoppingButton]");
            _checkoutButton = new Button(_page, GetBy.Role, "Checkout", $"{_pageName}_[CheckoutButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _cartList.CheckIsVisibleAsync();
                await _continueShoppingButton.CheckIsVisibleAsync();
                await _checkoutButton.CheckIsVisibleAsync();
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

        public static async Task<CartPage> InitAsync(IPage page)
        {
            CartPage cartPage = new(page);
            await cartPage.InitAsync();
            return cartPage;
        }

        public ILocator GetCartListLocator()
        {
            return _cartList.Locator;
        }

        public ILocator GetCartItemsLocator()
        {
            return _cartList.ListItemLocator;
        }

        public ILocator GetCartItemLocatorByOrdinalNumber(int ordinalNumber)
        {
            return _cartList.GetItemLocatorByOrdinalNumber(ordinalNumber);
        }

        public ILocator GetCartItemNameLocatorByOrdinalNumber(int ordinalNumber)
        {
            return _cartList.GetItemElementLocatorAsync(ordinalNumber, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetContinueShoppingButtonLocator()
        {
            return _continueShoppingButton.Locator;
        }

        public ILocator GetCheckoutButtonLocator()
        {
            return _checkoutButton.Locator;
        }

        public ILocator GetCartItemPriceLocatorByOrdinalNumber(int ordinalNumber)
        {
            return _cartList.GetItemElementLocatorAsync(ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<int> GetCartItemsCountAsync()
        {
            EnsureInitialized();
            return await _cartList.GetItemCountAsync();
        }

        public async Task<string> GetCartItemNameAsync(int ordinalNumber)
        {
            EnsureInitialized();
            return await _cartList.GetItemElementTextAsync(ordinalNumber, GetBy.CssSelector, "div.inventory_item_name");
        }

        public async Task<string> GetCartItemPriceAsync(int ordinalNumber)
        {
            EnsureInitialized();
            return await _cartList.GetItemElementTextAsync(ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<CartPage> RemoveCartItemAsync(int ordinalNumber)
        {
            EnsureInitialized();
            try
            {
                await _cartList.ClickOnItemElementAsync(ordinalNumber, "button");
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to remove cart item at ordinal number {ordinalNumber} within {_defaultTimeout} miliseconds.", ex);
            }
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> ClickContinueShoppingAsync()
        {
            EnsureInitialized();
            try
            {
                await _continueShoppingButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click Continue Shopping button within {_defaultTimeout} miliseconds.", ex);
            }
            return await ProductsPage.InitAsync(_page);
        }

        public async Task<CheckoutPage> ClickCheckoutAsync()
        {
            EnsureInitialized();
            try
            {
                await _checkoutButton.ClickAsync();
            }
            catch (TimeoutException ex)
            {
                throw new AssertionException($"[{_pageName}] Failed to click Checkout button within {_defaultTimeout} miliseconds.", ex);
            }
            return await CheckoutPage.InitAsync(_page);
        }
    }
}
