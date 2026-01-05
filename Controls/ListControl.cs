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

        public async Task AssertItemCountAsync(int expectedCount)
        {
            try
            {
                await Assertions.Expect(_listItemLocator).ToHaveCountAsync(expectedCount);
            }
            catch (PlaywrightException ex)
            {
                int actualCount = await _listItemLocator.CountAsync();
                throw new AssertionException($"List {_description} should contain {expectedCount} items, but contains {actualCount}", ex);
            }
        }

        public async Task AssertItemElementTextAsync(string expectedText, int ordinalNumber, GetBy getBy, string name, string elementDescription)
        {
            await CheckIfItemIsVisibleAsync(ordinalNumber);

            try
            {
                await Assertions.Expect(_listItemLocator.Nth(ordinalNumber).Locator(GetLocator(_page, getBy, AriaRole.None, name)))
                    .ToHaveTextAsync(expectedText);
            }
            catch (PlaywrightException ex)
            {
                string actualText = await _listItemLocator.Nth(ordinalNumber).Locator(GetLocator(_page, getBy, AriaRole.None, name))
                    .InnerTextAsync();
                throw new AssertionException(
                    $"{elementDescription}_{_listItemDescription}_{ordinalNumber} should have text '{expectedText}', but has '{actualText}'", 
                    ex
                    );
            }
        }

        public async Task ClickOnItemElementAsync(int ordinalNumber, string name)
        {
            await CheckIfItemIsVisibleAsync(ordinalNumber);
            await _listItemLocator.Nth(ordinalNumber).Locator(name).ClickAsync();
        }

        public async Task CheckIfItemIsVisibleAsync(int ordinalNumber, string message = "")
        {
            try
            {
                await Assertions.Expect(_listItemLocator.Nth(ordinalNumber)).ToBeVisibleAsync();
            }
            catch (PlaywrightException ex)
            {
                throw new AssertionException($"ListItem {_listItemName}_{ordinalNumber} is not visible. {message}", ex);
            }
        }

        public ILocator GetItemLocatorByOrdinalNumber(int ordinalNumber)
        {
            return _listItemLocator.Nth(ordinalNumber);
        }
    }
}
