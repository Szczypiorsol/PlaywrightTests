using Microsoft.Playwright;
using SwagLabs.Models;

namespace SwagLabs
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : BaseTest
    {
        [Test]
        public async Task T001_When_UserIsLockedOut_Should_DisplayErrorMessage()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            loginPage = await loginPage.LoginWithInvalidCredentialsAsync(Users["LockedOutUser"], "secret_sauce");
            await loginPage.AssertErrorMessageAsync("Epic sadface: Sorry, this user has been locked out.");
        }

        [Test]
        public async Task T002_When_UserEntersWrongPassword_Should_DisplayInvalidCredentialsMessage()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            
            loginPage = await loginPage.LoginWithInvalidCredentialsAsync(UserLogin, "wrong_password");
            await loginPage.AssertErrorMessageAsync("Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task T003_When_UserEntersWrongLogin_Should_DisplayInvalidCredentialsMessage()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            
            loginPage = await loginPage.LoginWithInvalidCredentialsAsync("admin_user", "secret_sauce");
            await loginPage.AssertErrorMessageAsync("Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task T004_When_SingleProductIsBought_Should_ValidateDetailsAndConfirmOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(1);
            await cartPage.AssertCartItemAsync(0, "Sauce Labs Bike Light", "$9.99");
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
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
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T005_When_UserSortsProductsByNameAndPrice_Should_DisplayCorrectOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            
            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            await productsPage.AssertProductsCountAsync(6);
            productsPage = await productsPage.SelectSortOptionAsync("Name (Z to A)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Onesie", "$7.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Fleece Jacket", "$49.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Sauce Labs Backpack", "$29.99");
            productsPage = await productsPage.SelectSortOptionAsync("Price (low to high)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Sauce Labs Onesie", "$7.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Backpack", "$29.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Sauce Labs Fleece Jacket", "$49.99");
            productsPage = await productsPage.SelectSortOptionAsync("Price (high to low)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Sauce Labs Fleece Jacket", "$49.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Backpack", "$29.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Sauce Labs Onesie", "$7.99");
            productsPage = await productsPage.SelectSortOptionAsync("Name (A to Z)");
            await productsPage.AssertProductByOrdinalNumberAsync(0, "Sauce Labs Backpack", "$29.99");
            await productsPage.AssertProductByOrdinalNumberAsync(1, "Sauce Labs Bike Light", "$9.99");
            await productsPage.AssertProductByOrdinalNumberAsync(2, "Sauce Labs Bolt T-Shirt", "$15.99");
            await productsPage.AssertProductByOrdinalNumberAsync(3, "Sauce Labs Fleece Jacket", "$49.99");
            await productsPage.AssertProductByOrdinalNumberAsync(4, "Sauce Labs Onesie", "$7.99");
            await productsPage.AssertProductByOrdinalNumberAsync(5, "Test.allTheThings() T-Shirt (Red)", "$15.99");
        }

        [Test]
        public async Task T006_When_SixProductsAreOrdered_Should_ValidateTotalsAndConfirmOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);
            
            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            await productsPage.AssertProductsCountAsync(6);
            for (int i = 0; i < 6; i++)
            {
                productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(i);
            }
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(6);
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(6);
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $129.94");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $10.40");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $140.34");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T007_When_ItemIsDeletedFromCartOnProductPage_Should_ReflectCorrectTotalsAndConfirmOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(0);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(2);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(4);
            productsPage = await productsPage.RemoveProductByOrdinalNumberAsync(2);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(2);
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(2);
            await checkoutOverviewPage.AssertOverviewItemAtAsync(0, "Sauce Labs Backpack", "$29.99");
            await checkoutOverviewPage.AssertOverviewItemAtAsync(1, "Sauce Labs Onesie", "$7.99");
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $37.98");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $3.04");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $41.02");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T008_When_ItemIsDeletedFromCartOnCartPage_Should_ReflectCorrectTotalsAndConfirmOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(3);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(5);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(3);
            cartPage = await cartPage.RemoveCartItemAsync(1);
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(2);
            await checkoutOverviewPage.AssertOverviewItemAtAsync(0, "Sauce Labs Bike Light", "$9.99");
            await checkoutOverviewPage.AssertOverviewItemAtAsync(1, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $25.98");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $2.08");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $28.06");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T009_When_CheckoutFormIsIncomplete_Should_DisplayValidationErrors()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(0);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(3);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(4);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(3);
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            await checkoutPage.ClickContinueAsync("Error: First Name is required");
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync(firstName: "John");
            await checkoutPage.ClickContinueAsync("Error: Last Name is required");
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync(lastName: "Doe");
            await checkoutPage.ClickContinueAsync("Error: Postal Code is required");
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync(postalCode: "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(3);
            await checkoutOverviewPage.AssertOverviewItemAtAsync(0, "Sauce Labs Backpack", "$29.99");
            await checkoutOverviewPage.AssertOverviewItemAtAsync(1, "Sauce Labs Fleece Jacket", "$49.99");
            await checkoutOverviewPage.AssertOverviewItemAtAsync(2, "Sauce Labs Onesie", "$7.99");
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $87.97");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $7.04");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $95.01");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T10_When_SameUserReauthentication_Should_PreserveCartContentsAndConfirmOrder()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(2);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(5);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(3);
            loginPage = await cartPage.LogoutAsync();
            productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(3);
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(3);
            await checkoutOverviewPage.AssertOverviewItemAtAsync(0, "Sauce Labs Bike Light", "$9.99");
            await checkoutOverviewPage.AssertOverviewItemAtAsync(1, "Sauce Labs Bolt T-Shirt", "$15.99");
            await checkoutOverviewPage.AssertOverviewItemAtAsync(2, "Test.allTheThings() T-Shirt (Red)", "$15.99");
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $41.97");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $3.36");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $45.33");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }

        [Test]
        public async Task T11_When_DifferentUserReauthentication_Should_NotRetainPreviousCart()
        {
            await PageInstance.GotoAsync("https://www.saucedemo.com/");
            LoginPage loginPage = await LoginPage.InitAsync(PageInstance);

            ProductsPage productsPage = await loginPage.LoginAsync(UserLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(1);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(2);
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(5);
            CartPage cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(3);
            loginPage = await cartPage.LogoutAsync();
            string userToLogin = UserLogin == Users["StandardUser"] ? Users["VisualUser"] : Users["StandardUser"];
            productsPage = await loginPage.LoginAsync(userToLogin, "secret_sauce");
            productsPage = await productsPage.ClickOnProductByOrdinalNumberAsync(4);
            cartPage = await productsPage.ClickOnCartButtonAsync();
            await cartPage.AssertCartItemsCountAsync(1);
            CheckoutPage checkoutPage = await cartPage.ClickCheckoutAsync();
            checkoutPage = await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            CheckoutOverviewPage checkoutOverviewPage = await checkoutPage.ClickContinueAsync();
            await checkoutOverviewPage.AssertOverviewItemsCountAsync(1);
            await checkoutOverviewPage.AssertOverviewItemAtAsync(0, "Sauce Labs Onesie", "$7.99");
            await checkoutOverviewPage.AssertPaymentInformationAsync("SauceCard #31337");
            await checkoutOverviewPage.AssertShippingInformationAsync("Free Pony Express Delivery!");
            await checkoutOverviewPage.AssertSummarySubtotalAsync("Item total: $7.99");
            await checkoutOverviewPage.AssertSummaryTaxAsync("Tax: $0.64");
            await checkoutOverviewPage.AssertSummaryTotalAsync("Total: $8.63");
            CheckoutCompletePage checkoutCompletePage = await checkoutOverviewPage.ClickFinishAsync();
            await checkoutCompletePage.AssertThankYouMessageAsync("Thank you for your order!");
            _ = await checkoutCompletePage.ClickBackHomeAsync();
        }
    }
}
