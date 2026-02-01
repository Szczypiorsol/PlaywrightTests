using Microsoft.Playwright;
using NUnit.Framework;

namespace Controls
{
    public class ListControl : Control
    {
        private readonly IPage _page;
        private readonly ILocator _listItemLocator;
        private readonly string _listItemName;
        private readonly string _listItemDescription;

        public ListControl(IPage page, GetBy getByList, string listName, string listDescription,
            GetBy getByItem, string itemName, string listItemDescription) 
            : base(GetLocator(page, getByList, AriaRole.List, listName), listName, listDescription)
        {
            if (string.IsNullOrEmpty(listItemDescription))
                throw new ArgumentException("ListItemDescription cannot be null or empty.", nameof(listItemDescription));

            ILocator listItemLocator = GetLocator(page, getByItem, AriaRole.Listitem, itemName);

            _listItemLocator = listItemLocator ?? throw new ArgumentException("ListItemLocator cannot be null.");

            _page = page;
            _listItemName = itemName;
            _listItemDescription = listItemDescription;
        }

        public ILocator ListItemLocator => _listItemLocator;

        public ILocator GetItemLocatorByOrdinalNumber(int ordinalNumber)
        {
            return _listItemLocator.Nth(ordinalNumber);
        }

        public ILocator GetItemElementLocatorAsync(int ordinalNumber, GetBy getBy, string name)
        {
            return GetItemLocatorByOrdinalNumber(ordinalNumber).Locator(GetLocator(_page, getBy, AriaRole.None, name));
        }

        public async Task<int> GetItemCountAsync()
        {
            return await _listItemLocator.CountAsync();
        }

        public async Task<string> GetItemElementTextAsync(int ordinalNumber, GetBy getBy, string name)
        {
            await GetIfItemIsVisibleAsync(ordinalNumber);

            return await GetItemLocatorByOrdinalNumber(ordinalNumber).Locator(GetLocator(_page, getBy, AriaRole.None, name)).InnerTextAsync();
        }

        public async Task ClickOnItemElementAsync(int ordinalNumber, string name)
        {
            await GetIfItemIsVisibleAsync(ordinalNumber);
            await GetItemLocatorByOrdinalNumber(ordinalNumber).Locator(name).ClickAsync();
        }

        public async Task<bool> GetIfItemIsVisibleAsync(int ordinalNumber)
        {
            return await GetItemLocatorByOrdinalNumber(ordinalNumber).IsVisibleAsync();
        }
    }
}
