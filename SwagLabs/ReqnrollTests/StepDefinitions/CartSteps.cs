using Microsoft.Playwright;
using Reqnroll;
using Tests.SwagLabs.Pages;
using NUnit.Framework;

namespace Tests.SwagLabs.ReqnrollTests.StepDefinitions
{
    [Binding]
    internal class CartSteps(ScenarioContext scenarioContext)
    {
        private readonly CartPage? _cartPage = new(scenarioContext.Get<IPage>());

        [When(@"Usuwa produkt ""(.*)"" z koszyka")]
        public async Task RemoveProductFromCart(string productName)
        {
            await _cartPage.InitAsync();
            await _cartPage.ClickRemoveProductByNameAsync(productName);
        }

        [When(@"Rozpoczyna realizację zamówienia")]
        public async Task StartCheckout()
        {
            await _cartPage.InitAsync();
            await _cartPage.ClickCheckoutAsync();
        }

        [Then(@"Koszyk powinien zawierać produkt ""(.*)""")]
        public async Task CartShouldHave(string productName)
        {
            await _cartPage.InitAsync();
            bool isProductInCart = await _cartPage.IsProductInCartAsync(productName);
            Assert.IsTrue(isProductInCart, $"Expected product '{productName}' to be in the cart, but it was not found.");
        }

        [Then(@"Koszyk powinien być pusty")]
        public async Task CartShouldBeEmpty()
        {
            await _cartPage.InitAsync();
            bool isCartEmpty = await _cartPage.ProductsListControl.ItemsLocator.CountAsync() == 0;
            Assert.IsTrue(isCartEmpty, "Expected the cart to be empty, but it was not.");
        }
    }
}
