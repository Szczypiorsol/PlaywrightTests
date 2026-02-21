using Microsoft.Playwright;
using Reqnroll;
using Tests.SwagLabs.Pages;

namespace Tests.SwagLabs.ReqnrollTests.StepDefinitions
{
    [Binding]
    internal class ProductsSteps(ScenarioContext scenarioContext)
    {
        private readonly ProductsPage? _productsPage = new(scenarioContext.Get<IPage>());

        [When(@"Wybiera sortowanie ""(.*)""")]
        public async Task SelectSorting(string sortingOption)
        {
            await _productsPage.InitAsync();
            await _productsPage.SelectSortOptionAsync(sortingOption);
        }

        [When(@"Dodaje produkt ""(.*)"" do koszyka")]
        public async Task AddProductToCart(string productName)
        {
            await _productsPage.InitAsync();
            await _productsPage.ClickOnProductByNameAsync(productName);
        }

        [When(@"Dodaje produkty do koszyka: (.*)")]
        public async Task AddProductsToCart(string ProductList)
        {
            string[] productNames = [.. ProductList.Replace('"', ' ').Split(',').Select(p => p.Trim())];

            await _productsPage.InitAsync();
            foreach (string productName in productNames)
            {
                await _productsPage.ClickOnProductByNameAsync(productName);
            }
        }

        [When(@"Przechodzi do koszyka")]
        public async Task GoToCart()
        {
            await _productsPage.InitAsync();
            await _productsPage.ClickOnCartButtonAsync();
        }

        [Then(@"Produkty powinny być posortowane zgodnie z ""(.*)""")]
        public async Task ProductsShouldBeSorted(string sortingOption)
        {
            await _productsPage.InitAsync();
            bool isSortedCorrectly = await _productsPage.AreProductsSortedAsync(sortingOption);
            Assert.IsTrue(isSortedCorrectly, $"Expected products to be sorted by '{sortingOption}', but they were not.");
        }

        [Then(@"Licznik koszyka powinien wynosić (.*)")]
        public async Task CartCountShouldBe(int expectedCount)
        {
            await _productsPage.InitAsync();
            string actualCountText = await _productsPage.NumberOfProductsInCartTextBox.GetTextAsync();
            int actualCount = int.TryParse(actualCountText, out int count) ? count : 0;
            Assert.AreEqual(expectedCount, actualCount, $"Expected cart count to be {expectedCount}, but was {actualCount}.");
        }
    }
}
