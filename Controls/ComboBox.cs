using Microsoft.Playwright;
using NUnit.Framework;

namespace Controls
{
    public class ComboBox : Control
    {
        private readonly ILocator _comboBoxItemLocator;
        private readonly string _comboBoxItemName;
        private readonly string _comboBoxItemDescription;

        public ComboBox(IPage page, GetBy getByList, string comboBoxName, string description,
            GetBy getByItem, string itemName, string itemDescription) 
            : base(GetLocator(page, getByList, AriaRole.List, comboBoxName), comboBoxName, description)
        {
            if (string.IsNullOrEmpty(itemDescription))
                throw new ArgumentException("ItemDescription cannot be null or empty.", nameof(itemDescription));

            ILocator comboBoxItemLocator = GetLocator(page, getByItem, AriaRole.Listitem, itemName);
            
            _comboBoxItemLocator = comboBoxItemLocator ?? throw new ArgumentException("ComboBoxItemLocator cannot be null.");
            _comboBoxItemName = itemName;
            _comboBoxItemDescription = itemDescription;
        }

        public async Task SelectItemByTextAsync(string itemText)
        {
            await _locator.SelectOptionAsync(new SelectOptionValue { Label = itemText });
        }

        public async Task CheckIfItemIsVisibleAsync(int ordinalNumber)
        {
            try
            {
                await _comboBoxItemLocator.Nth(ordinalNumber).IsVisibleAsync();
            }
            catch
            {
                throw new AssertionException($"ComboBoxItem {_comboBoxItemDescription}_{ordinalNumber} is not visible.");
            }
        }
    }
}
