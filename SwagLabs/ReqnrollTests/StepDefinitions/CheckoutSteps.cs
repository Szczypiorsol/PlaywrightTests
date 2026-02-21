using Microsoft.Playwright;
using Reqnroll;
using Tests.SwagLabs.Pages;

namespace Tests.SwagLabs.ReqnrollTests.StepDefinitions
{
    [Binding]
    internal class CheckoutSteps(ScenarioContext scenarioContext)
    {
        private readonly CheckoutPage? _checkoutPage = new(scenarioContext.Get<IPage>());

        [When(@"Podaje dane klienta: imię ""(.*)"", nazwisko ""(.*)"", kod ""(.*)""")]
        public async Task EnterCheckoutInformation(string firstName, string lastName, string postalCode)
        {
            await _checkoutPage.InitAsync();
            await _checkoutPage.FillCheckoutInformationAsync(firstName, lastName, postalCode);
        }

        [When(@"Próbuje przejść do podsumowania zamówienia")]
        public async Task TryGoToCheckoutOverview()
        {
            await _checkoutPage.InitAsync();
            await _checkoutPage.ClickContinueErrorExpectedAsync();
        }

        [When(@"Przechodzi do podsumowania zamówienia")]
        public async Task GoToCheckoutOverview()
        {
            await _checkoutPage.InitAsync();
            await _checkoutPage.ClickContinueAsync();
        }

        [Then(@"Powinien zobaczyć komunikat walidacyjny ""(.*)""")]
        public async Task ShouldSeeValidationMessage(string expectedMessage)
        {
            await _checkoutPage.InitAsync();
            string actualMessage = await _checkoutPage.ErrorMessageTextBox.GetTextAsync();
            Assert.AreEqual(expectedMessage, actualMessage, $"Expected validation message to be '{expectedMessage}', but was '{actualMessage}'.");
        }
    }
}
