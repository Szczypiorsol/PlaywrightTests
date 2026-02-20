using Microsoft.Playwright;
using Tests.Infrastructure;
using Tests.SwagLabs.Controls;
using static Tests.SwagLabs.Controls.Control;

namespace Tests.SwagLabs.Pages
{
    public class CartPage : BasePage
    {
        private readonly ListControl _productsListControl;
        private readonly Button _continueShoppingButton;
        private readonly Button _checkoutButton;

        public ListControl ProductsListControl => _productsListControl;
        public Button ContinueShoppingButton => _continueShoppingButton;
        public Button CheckoutButton => _checkoutButton;

        public CartPage(IPage page) : base(page, "CartPage")
        {
            _productsListControl = new ListControl(
                page: _page,
                getByList: GetBy.CssSelector,
                listName: "div.cart_list",
                getByItem: GetBy.CssSelector,
                itemName: "div.cart_item"
                );
            _continueShoppingButton = new Button(_page, GetBy.Role, "Continue Shopping");
            _checkoutButton = new Button(_page, GetBy.Role, "Checkout");
        }

        public override async Task InitAsync()
        {
            TestsLogger.LogInformation("Initializing [CartPage]...");
            await ProductsListControl.WaitToBeVisibleAsync();
            await ContinueShoppingButton.WaitToBeVisibleAsync();
            await CheckoutButton.WaitToBeVisibleAsync();

            _isInitialized = true;
            TestsLogger.LogDebug("[CartPage] initialized successfully.");
        }

        public static async Task<CartPage> InitAsync(IPage page)
        {
            CartPage cartPage = new(page);
            await cartPage.InitAsync();
            return cartPage;
        }

        public ILocator GetProductNameLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetProductPriceLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<CartPage> RemoveCartItemAsync(int ordinalNumber)
        {
            TestsLogger.LogInformation("Removing item #{OrdinalNumber} from the cart...", ordinalNumber);
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            TestsLogger.LogDebug("Item #{OrdinalNumber} removed from the cart.", ordinalNumber);
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> ClickContinueShoppingAsync()
        {
            TestsLogger.LogInformation("Clicking 'Continue Shopping' button...");
            EnsureInitialized();
            await ContinueShoppingButton.ClickAsync();
            TestsLogger.LogDebug("'Continue Shopping' button clicked.");
            return await ProductsPage.InitAsync(_page);
        }

        public async Task<CheckoutPage> ClickCheckoutAsync()
        {
            TestsLogger.LogInformation("Clicking 'Checkout' button...");
            EnsureInitialized();
            await CheckoutButton.ClickAsync();
            TestsLogger.LogDebug("'Checkout' button clicked.");
            return await CheckoutPage.InitAsync(_page);
        }
    }
}
