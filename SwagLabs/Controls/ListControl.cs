using Microsoft.Playwright;

namespace Tests.SwagLabs.Controls
{
    public class ListControl : Control
    {
        private readonly IPage _page;
        private readonly ILocator _itemsLocator;

        public ILocator ItemsLocator => _itemsLocator;

        public ListControl(IPage page, GetBy getByList, string listName, GetBy getByItem, string itemName) 
            : base(GetLocator(page, getByList, listName, AriaRole.List))
        {
            ILocator listItemLocator = GetLocator(page, getByItem, itemName, AriaRole.Listitem);

            _itemsLocator = listItemLocator ?? throw new ArgumentException("ListItemLocator cannot be null.");

            _page = page;
        }

        public int GetItemOrdinalNumberByElementText(string elementText)
        {
            int itemCount = ItemsLocator.CountAsync().GetAwaiter().GetResult();
            for(int i = 0; i < itemCount; i++)
            {
                string text = ItemsLocator.Nth(i).InnerTextAsync().GetAwaiter().GetResult();
                if (text.Trim() == elementText)
                {
                    return i;
                }
            }
            return -1;
        }

        public ILocator GetItemElementLocator(int ordinalNumber, GetBy getBy, string name)
        {
            return ItemsLocator.Nth(ordinalNumber).Locator(GetLocator(_page, getBy, name));
        }

        public async Task ClickOnItemElementAsync(int ordinalNumber, string name)
        {
            await ItemsLocator.Nth(ordinalNumber).Locator(name).ClickAsync();
        }
    }
}
