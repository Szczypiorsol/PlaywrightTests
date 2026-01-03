using Microsoft.Playwright;
using static Controls.Control;

namespace Controls
{
    public class ListControl(IPage page, GetBy getByList, string listName, GetBy getByItem, string itemName) : Control(GetLocator(page, getByList, AriaRole.List, listName), listName)
    {
        private readonly IPage _page = page;
        private readonly ILocator _listItemLocator = GetLocator(page, getByItem, AriaRole.Listitem, itemName);
        private readonly string _listItemName = itemName;

        public async Task AssertItemCountAsync(int expectedCount)
        {
            await Assertions.Expect(_listItemLocator).ToHaveCountAsync(expectedCount);
        }

        public async Task AssertItemElementTextAsync(string expectedText, int ordinalNumber, GetBy getBy, string name)
        {
            await CheckIfItemIsVisibleAsync(ordinalNumber);
            await Assertions.Expect(_listItemLocator.Nth(ordinalNumber).Locator(GetLocator(_page, getBy, AriaRole.None, name)))
                .ToHaveTextAsync(expectedText);
        }

        public async Task ClickOnItemElementAsync(int ordinalNumber, string name)
        {
            await CheckIfItemIsVisibleAsync(ordinalNumber);
            await _listItemLocator.Nth(ordinalNumber).Locator(name).ClickAsync();
        }

        public async Task CheckIfItemIsVisibleAsync(int ordinalNumber)
        {
            if (_listItemLocator.Nth(ordinalNumber).IsVisibleAsync().GetAwaiter().GetResult() != true)
            {
                throw new Exception($"ListItem {_listItemName}_{ordinalNumber} is not visible.");
            }
        }

        public ILocator GetItemLocatorByOrdinalNumber(int ordinalNumber)
        {
            return _listItemLocator.Nth(ordinalNumber);
        }
    }
}
