using Microsoft.Playwright;
using NUnit.Framework;

namespace Controls
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

        protected readonly ILocator _locator;
        protected readonly string _name;
        protected readonly string _description;

        public ILocator Locator => _locator;

        public Control(ILocator locator, string name, string description)
        {
            if (locator is null)
            {
                throw new ArgumentNullException(nameof(locator), "Locator cannot be null.");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Description cannot be null or empty.", nameof(description));
            }

            _locator = locator;
            _name = name;
            _description = description;
        }

        public static ILocator GetLocator(IPage page, GetBy getBy, AriaRole ariaRole, string name)
        {
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

        public async Task CheckIsVisibleAsync()
        {
            try
            {
                await Assertions.Expect(_locator).ToBeVisibleAsync();
            }
            catch (PlaywrightException ex)
            {
                string TypeName = this.GetType().Name;
                throw new AssertionException($"{TypeName} {_description} is not visible.", ex);
            }
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
