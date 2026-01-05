using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Models
{
    public class LoginPage : BasePage
    {
        private readonly TextBox _usernameTextBox;
        private readonly TextBox _passwordTextBox;
        private readonly Button _loginButton;

        public LoginPage(IPage page) : base(page, "[LoginPage]")
        {
            _usernameTextBox = new TextBox(_page, GetBy.Role, "Username", $"{_pageName}_[UsernameTextBox]");
            _passwordTextBox = new TextBox(_page, GetBy.Role, "Password", $"{_pageName}_[PasswordTextBox]");
            _loginButton = new Button(_page, GetBy.Role, "Login", $"{_pageName}_[LoginButton]");
        }

        public override async Task InitAsync()
        {
            try
            {
                await _usernameTextBox.CheckIsVisibleAsync();
                await _passwordTextBox.CheckIsVisibleAsync();
                await _loginButton.CheckIsVisibleAsync();
            }
            catch (AssertionException ex)
            {
                throw new AssertionException($"{_pageName} did not load correctly.", ex);
            }

            _isInitialized = true;
        }

        public static async Task<LoginPage> InitAsync(IPage page)
        {
            LoginPage loginPage = new(page);
            await loginPage.InitAsync();
            return loginPage;
        }

        public async Task<ProductsPage> LoginAsync(string username, string password)
        {
            EnsureInitialized();
            await _usernameTextBox.EnterTextAsync(username);
            await _passwordTextBox.EnterTextAsync(password);
            await _loginButton.ClickAsync();
            return await ProductsPage.InitAsync(_page);
        }

        public async Task<LoginPage> LoginWithInvalidCredentialsAsync(string username, string password)
        {
            EnsureInitialized();
            await _usernameTextBox.EnterTextAsync(username);
            await _passwordTextBox.EnterTextAsync(password);
            await _loginButton.ClickAsync();
            return await InitAsync(_page);
        }
    }
}