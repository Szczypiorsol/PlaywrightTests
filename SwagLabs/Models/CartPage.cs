using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class CartPage : BasePage
    {
        private readonly ListControl _cartList;
        private readonly Button _continueShoppingButton;
        private readonly Button _checkoutButton;

        public CartPage(IPage page) : base(page, "[CartPage]")
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

            _isInitialized = true;
        }

        public static async Task<CartPage> InitAsync(IPage page)
        {
            CartPage cartPage = new(page);
            await cartPage.InitAsync();
            return cartPage;
        }

        public async Task AssertCartItemsCountAsync(int expectedCount)
        {
            EnsureInitialized();
            await _cartList.AssertItemCountAsync(expectedCount);
        }

        public async Task AssertCartItemAsync(int ordinalNumber, string expecterCartItemName, string expecterCartItemPrice)
        {
            EnsureInitialized();
            await _cartList.AssertItemElementTextAsync(expecterCartItemName, ordinalNumber, GetBy.CssSelector, "div.inventory_item_name", "Name");
            await _cartList.AssertItemElementTextAsync(
                expecterCartItemPrice, 
                ordinalNumber, 
                GetBy.CssSelector, 
                "div.inventory_item_price", 
                "Price"
                );
        }

        public async Task RemoveCartItemAsync(int ordinalNumber)
        {
            EnsureInitialized();
            await _cartList.ClickOnItemElementAsync(ordinalNumber, "button");
        }

        public async Task ClickContinueShoppingAsync()
        {
            EnsureInitialized();
            await _continueShoppingButton.ClickAsync();
        }

        public async Task<CheckoutPage> ClickCheckoutAsync()
        {
            EnsureInitialized();
            await _checkoutButton.ClickAsync();
            return await CheckoutPage.InitAsync(_page);
        }
    }
}
