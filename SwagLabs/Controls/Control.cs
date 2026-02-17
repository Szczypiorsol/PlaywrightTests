using Microsoft.Playwright;

namespace Tests.SwagLabs.Controls
{
    public class Control
    {
        public enum GetBy
        {
            Role,
            TestId,
            Text,
            CssSelector,
            Placeholder
        }

        private readonly ILocator _locator;

        public ILocator Locator => _locator;

        public Control(ILocator locator)
        {
            if (locator is null)
            {
                throw new ArgumentNullException(nameof(locator), "Locator cannot be null.");
            }

            _locator = locator;
        }

        public static ILocator GetLocator(IPage page, GetBy getBy, string name, AriaRole ariaRole = AriaRole.None)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page), "Page cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }

            return getBy switch
            {
                GetBy.Role => page.GetByRole(ariaRole, new PageGetByRoleOptions { Name = name }),
                GetBy.TestId => page.Locator($"[data-test='{name}']"),
                GetBy.Text => page.GetByText(name),
                GetBy.CssSelector => page.Locator(name),
                GetBy.Placeholder => page.GetByPlaceholder(name),
                _ => throw new Exception("GetBy method not recognized."),
            };
        }

        public async Task WaitToBeVisibleAsync(int timeout = 3000)
        {
            await _locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeout
            });
        }

        public async Task ClickAsync()
        {
            await _locator.ClickAsync();
        }
    }
}
