using Microsoft.Playwright;
using Tests.Infrastructure;
using Tests.SwagLabs.Controls;
using static Tests.SwagLabs.Controls.Control;

namespace Tests.SwagLabs.Pages
{
    public class ProductsPage : BasePage
    {
        private readonly Button _cartButton;
        private readonly ComboBox _sortComboBox;
        private readonly ListControl _productsListControl;
        private readonly TextBox _numberOfProductsInCartTextBox;

        public Button CartButton => _cartButton;
        public ComboBox SortComboBox => _sortComboBox;
        public ListControl ProductsListControl => _productsListControl;
        public TextBox NumberOfProductsInCartTextBox => _numberOfProductsInCartTextBox;

        public ProductsPage(IPage page) : base(page, "[ProductsPage]")
        {
            _cartButton = new Button(_page, GetBy.CssSelector, "a.shopping_cart_link");
            _sortComboBox = new ComboBox(_page, GetBy.CssSelector, "select.product_sort_container", GetBy.CssSelector, "option");
            _productsListControl = new ListControl(_page, GetBy.CssSelector, "div.inventory_list", GetBy.CssSelector, "div.inventory_item");
            _numberOfProductsInCartTextBox = new TextBox(_page, GetBy.CssSelector, "span.shopping_cart_badge");
        }

        public override async Task InitAsync()
        {
            TestsLogger.LogInformation("Initializing [ProductsPage]...");
            await CartButton.WaitToBeVisibleAsync();
            await SortComboBox.WaitToBeVisibleAsync();
            await ProductsListControl.WaitToBeVisibleAsync();

            _isInitialized = true;
            TestsLogger.LogDebug("[ProductsPage] initialized successfully.");
        }

        public static async Task<ProductsPage> InitAsync(IPage page)
        {
            ProductsPage productsPage = new(page);
            await productsPage.InitAsync();
            return productsPage;
        }

        public ILocator GetProductNameLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_name");
        }

        public ILocator GetProductPriceLocator(int ordinalNumber)
        {
            return ProductsListControl.GetItemElementLocator(ordinalNumber, GetBy.CssSelector, "div.inventory_item_price");
        }

        public async Task<ProductsPage> ClickOnProductByOrdinalNumberAsync(int ordinalNumber)
        {
            TestsLogger.LogInformation("Clicking on product button at ordinal number {OrdinalNumber}...", ordinalNumber);
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            TestsLogger.LogDebug("Clicked on product button at ordinal number {OrdinalNumber}.", ordinalNumber);
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> ClickOnProductByNameAsync(string productName)
        {
            TestsLogger.LogInformation("Adding product at ordinal number {productName} to cart...", productName);
            EnsureInitialized();

            int productsCount = await ProductsListControl.ItemsLocator.CountAsync();
            for (int i = 0; i < productsCount; i++)
            {
                string currentProductName = await GetProductNameLocator(i).InnerTextAsync();
                if (currentProductName.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    await ProductsListControl.ClickOnItemElementAsync(i, "button");
                    TestsLogger.LogDebug("Added product '{ProductName}' at ordinal number {OrdinalNumber} to cart.", productName, i);
                    return await InitAsync(_page);
                }
            }

            TestsLogger.LogError("Product with name '{ProductName}' not found in the products list.", productName);
            throw new ArgumentException($"Product with name '{productName}' not found in the products list.");
        }

        public async Task<ProductsPage> RemoveProductByOrdinalNumberAsync(int ordinalNumber)
        {
            TestsLogger.LogInformation("Removing product at ordinal number {OrdinalNumber}...", ordinalNumber);
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            TestsLogger.LogDebug("Removed product at ordinal number {OrdinalNumber}.", ordinalNumber);
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> SelectSortOptionAsync(string optionText)
        {
            TestsLogger.LogInformation("Selecting sort option '{OptionText}'...", optionText);
            EnsureInitialized();
            await SortComboBox.SelectItemByTextAsync(optionText);
            TestsLogger.LogDebug("Selected sort option '{OptionText}'.", optionText);
            return await InitAsync(_page);
        }

        public async Task<bool> AreProductsSortedAsync(string sortingOption)
        {
            TestsLogger.LogInformation("Checking if products are sorted by '{SortingOption}'...", sortingOption);
            EnsureInitialized();
            int productsCount = await ProductsListControl.ItemsLocator.CountAsync();
            List<string> productNames = [];
            List<decimal> productPrices = [];
            for (int i = 0; i < productsCount; i++)
            {
                string name = await GetProductNameLocator(i).InnerTextAsync();
                string priceText = await GetProductPriceLocator(i).InnerTextAsync();
                decimal price = decimal.TryParse(priceText.Replace("$", "").Trim(), out decimal parsedPrice) ? parsedPrice : 0;
                productNames.Add(name);
                productPrices.Add(price);
            }
            bool isSorted = sortingOption switch
            {
                "Name (A to Z)" => productNames.SequenceEqual(productNames.OrderBy(n => n)),
                "Name (Z to A)" => productNames.SequenceEqual(productNames.OrderByDescending(n => n)),
                "Price (low to high)" => productPrices.SequenceEqual(productPrices.OrderBy(p => p)),
                "Price (high to low)" => productPrices.SequenceEqual(productPrices.OrderByDescending(p => p)),
                _ => throw new ArgumentException($"Unknown sorting option: {sortingOption}")
            };
            TestsLogger.LogDebug("Products are sorted by '{SortingOption}': {IsSorted}.", sortingOption, isSorted);
            return isSorted;
        }

        public async Task<CartPage> ClickOnCartButtonAsync()
        {
            TestsLogger.LogInformation("Clicking on Cart button...");
            EnsureInitialized();
            await CartButton.ClickAsync();
            TestsLogger.LogDebug("Clicked on Cart button.");
            return await CartPage.InitAsync(_page);
        }
    }
}
