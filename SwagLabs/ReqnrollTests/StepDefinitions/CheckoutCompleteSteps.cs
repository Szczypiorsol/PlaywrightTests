using Microsoft.Playwright;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.SwagLabs.Pages;

namespace Tests.SwagLabs.ReqnrollTests.StepDefinitions
{
    [Binding]
    internal class CheckoutCompleteSteps(ScenarioContext scenarioContext)
    {
        private CheckoutCompletePage? _checkoutCompletePage = new(scenarioContext.Get<IPage>());

        [Then(@"Powinien zobaczyć potwierdzenie zamówienia ""(.*)""")]
        public async Task VerifyOrderConfirmation(string expectedMessage)
        {
            await _checkoutCompletePage.InitAsync();
            string actualMessage = await _checkoutCompletePage.ThankYouMessageTextBox.GetTextAsync();
            if (actualMessage != expectedMessage)
            {
                throw new Exception($"Expected confirmation message to be '{expectedMessage}', but got '{actualMessage}'.");
            }
        }
    }
}
