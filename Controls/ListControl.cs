using Microsoft.Playwright;

namespace Controls
{
    public class ListControl : Control
    {
        private readonly IPage _page;
        private readonly ILocator _itemsLocator;

        public ILocator ItemsLocator => _itemsLocator;

        public ListControl(IPage page, GetBy getByList, string listName, GetBy getByItem, string itemName) 
            : base(GetLocator(page, getByList, AriaRole.List, listName))
        {
            ILocator listItemLocator = GetLocator(page, getByItem, AriaRole.Listitem, itemName);

            _itemsLocator = listItemLocator ?? throw new ArgumentException("ListItemLocator cannot be null.");

            _page = page;
        }

        public ILocator GetItemElementLocator(int ordinalNumber, GetBy getBy, string name)
        {
            return ItemsLocator.Nth(ordinalNumber).Locator(GetLocator(_page, getBy, AriaRole.None, name));
        }

        public async Task ClickOnItemElementAsync(int ordinalNumber, string name)
        {
            await ItemsLocator.Nth(ordinalNumber).Locator(name).ClickAsync();
        }
    }
}
