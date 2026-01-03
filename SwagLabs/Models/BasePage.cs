using Microsoft.Playwright;

namespace SwagLabs.Models
{
    public abstract class BasePage(IPage page)
    {
        protected readonly IPage _page = page ?? throw new ArgumentNullException(nameof(page));
        protected bool _isInitialized = false;

        public abstract Task InitAsync();

        protected void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Page is not initialized. Call InitAsync() before using the page.");
            }
        }
    }
}
