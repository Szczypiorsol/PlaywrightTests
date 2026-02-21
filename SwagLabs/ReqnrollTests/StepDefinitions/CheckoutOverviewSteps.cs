using Microsoft.Playwright;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.SwagLabs.Pages;

namespace Tests.SwagLabs.ReqnrollTests.StepDefinitions
{
    [Binding]
    internal class CheckoutOverviewSteps(ScenarioContext scenarioContext)
    {
        private readonly CheckoutOverviewPage? _checkoutOverviewPage = new(scenarioContext.Get<IPage>());

        [When(@"Finalizuje zamówienie")]
        public async Task FinishCheckout()
        {
            await _checkoutOverviewPage.InitAsync();
            await _checkoutOverviewPage.ClickFinishAsync();
        }
    }
}
