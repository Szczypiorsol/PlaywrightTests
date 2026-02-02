using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Pages
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
            await CartButton.WaitToBeVisibleAsync();
            await SortComboBox.WaitToBeVisibleAsync();
            await ProductsListControl.WaitToBeVisibleAsync();

            _isInitialized = true;
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
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> RemoveProductByOrdinalNumberAsync(int ordinalNumber)
        {
            EnsureInitialized();
            await ProductsListControl.ClickOnItemElementAsync(ordinalNumber, "button");
            return await InitAsync(_page);
        }

        public async Task<ProductsPage> SelectSortOptionAsync(string optionText)
        {
            EnsureInitialized();
            await SortComboBox.SelectItemByTextAsync(optionText);
            return await InitAsync(_page);
        }

        public async Task<CartPage> ClickOnCartButtonAsync()
        {
            EnsureInitialized();
            await CartButton.ClickAsync();
            return await CartPage.InitAsync(_page);
        }
    }
}
