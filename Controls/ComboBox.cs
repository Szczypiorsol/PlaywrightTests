using Microsoft.Playwright;

namespace Controls
{
    public class ComboBox : Control
    {
        private readonly ILocator _itemsLocator;

        public ILocator ItemsLocator => _itemsLocator;

        public ComboBox(IPage page, GetBy getByList, string comboBoxName, GetBy getByItem, string itemName) 
            : base(GetLocator(page, getByList, comboBoxName, AriaRole.List))
        {
            ILocator comboBoxItemLocator = GetLocator(page, getByItem, itemName, AriaRole.Listitem);
            
            _itemsLocator = comboBoxItemLocator ?? throw new ArgumentException("ComboBoxItemLocator cannot be null.");
        }

        public async Task SelectItemByTextAsync(string itemText)
        {
            await Locator.SelectOptionAsync(new SelectOptionValue { Label = itemText });
        }
    }
}
