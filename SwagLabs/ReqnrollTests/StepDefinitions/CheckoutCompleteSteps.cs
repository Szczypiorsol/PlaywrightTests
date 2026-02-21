using Microsoft.Playwright;
using Reqnroll;
using Tests.SwagLabs.Pages;

namespace Tests.SwagLabs.ReqnrollTests.StepDefinitions
{
    [Binding]
    internal class CheckoutCompleteSteps(ScenarioContext scenarioContext)
    {
        private readonly CheckoutCompletePage? _checkoutCompletePage = new(scenarioContext.Get<IPage>());

        [Then(@"Powinien zobaczyć potwierdzenie zamówienia ""(.*)""")]
        public async Task VerifyOrderConfirmation(string expectedMessage)
        {
            await _checkoutCompletePage.InitAsync();
            string actualMessage = await _checkoutCompletePage.ThankYouMessageTextBox.GetTextAsync();
            Assert.AreEqual(expectedMessage, actualMessage, $"Expected confirmation message to be '{expectedMessage}', but got '{actualMessage}'.");
        }
    }
}
