using Controls;
using Microsoft.Playwright;
using static Controls.Control;

namespace SwagLabs.Pages
{
    public class LoginPage : BasePage
    {
        private readonly TextBox _usernameTextBox;
        private readonly TextBox _passwordTextBox;
        private readonly TextBox _errorMessageTextBox;
        private readonly Button _loginButton;

        public TextBox UsernameTextBox => _usernameTextBox;
        public TextBox PasswordTextBox => _passwordTextBox;
        public TextBox ErrorMessageTextBox => _errorMessageTextBox;
        public Button LoginButton => _loginButton;

        public LoginPage(IPage page) : base(page, "LoginPage")
        {
            _usernameTextBox = new TextBox(_page, GetBy.Role, "Username");
            _passwordTextBox = new TextBox(_page, GetBy.Role, "Password");
            _errorMessageTextBox = new TextBox(_page, GetBy.CssSelector, "div.error-message-container");
            _loginButton = new Button(_page, GetBy.Role, "Login");
        }

        public override async Task InitAsync()
        {
            await UsernameTextBox.WaitToBeVisibleAsync();
            await PasswordTextBox.WaitToBeVisibleAsync();
            await LoginButton.WaitToBeVisibleAsync();

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
            await UsernameTextBox.EnterTextAsync(username);
            await PasswordTextBox.EnterTextAsync(password);
            await ClickLoginButton();
            return await ProductsPage.InitAsync(_page);
        }

        public async Task<LoginPage> LoginWithInvalidCredentialsAsync(string username, string password)
        {
            EnsureInitialized();
            await UsernameTextBox.EnterTextAsync(username);
            await PasswordTextBox.EnterTextAsync(password);
            await ClickLoginButton();
            return await InitAsync(_page);
        }

        private async Task ClickLoginButton()
        {
            await LoginButton.ClickAsync();
        }
    }
}