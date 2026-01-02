using Microsoft.Playwright;
using SwagLabs.Models;

namespace SwagLabs
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [TestCase("standard_user")]
        [TestCase("locked_out_user")]
        [TestCase("problem_user")]
        [TestCase("performance_glitch_user")]
        [TestCase("error_user")]
        [TestCase("visual_user")]
        public async Task SellItemPositivePath(string login)
        {
            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            LoginPage loginPage = await LoginPage.InitAsync(page);
            ProductsPage productsPage = await loginPage.LoginAsync(login, "secret_sauce");
            await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(1);
            await cartPage.AssertCartItemAsync(0, "Sauce Labs Bike Light", "$9.99");
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(1);
            await checkoutOverviewPage.AssertOverviewItemAtAsync(0, "Sauce Labs Bike Light", "$9.99");
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $9.99");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $0.80");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $10.79");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            productsPage = await checkoutCompletePage.ClickBackHomeAsync();
        }
    }
}
