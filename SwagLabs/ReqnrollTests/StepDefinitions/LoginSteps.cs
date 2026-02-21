using Microsoft.Playwright;
using Reqnroll;
using Tests.SwagLabs.Pages;

namespace Tests.SwagLabs.ReqnrollTests.StepDefinitions
{
    [Binding]
    internal class LoginSteps(ScenarioContext scenarioContext)
    {
        private readonly IPage _page = scenarioContext.Get<IPage>("PageInstance");
        private LoginPage? _loginPage;

        [Given(@"Użytkownik jest na stronie logowania Swag Labs")]
        public async Task UserIsOnLoginPage()
        {
            await _page.GotoAsync("https://www.saucedemo.com/");
            _loginPage = await LoginPage.InitAsync(_page);
        }

        [When(@"Loguje się jako ""(.*)"" z hasłem ""(.*)""")]
        public async Task LoginUser(string username, string password)
        {
            if (_loginPage == null)
            {
                throw new InvalidOperationException("LoginPage is not initialized. Ensure that the user is on the login page before attempting to log in.");
            }
            try
            {
                await _loginPage.LoginAsync(username, password);
            }
            catch (Exception) {}
        }

        [Then(@"Powinien zobaczyć komunikat błędu ""(.*)""")]
        public async Task ShouldSeeErrorMessage(string expectedErrorMessage)
        {
            if (_loginPage == null)
            {
                throw new InvalidOperationException("LoginPage is not initialized. Ensure that the user is on the login page before checking for error messages.");
            }
            string actualErrorMessage = await _loginPage.ErrorMessageTextBox.GetTextAsync();
            Assert.AreEqual(expectedErrorMessage, actualErrorMessage, $"Expected error message to be '{expectedErrorMessage}', but was '{actualErrorMessage}'.");
        }
    }
}
