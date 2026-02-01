using Microsoft.Playwright;
using NUnit.Framework;

namespace Controls
{
    public class TextBox(IPage page, Control.GetBy getBy, string name, string description) 
        : Control(GetLocator(page, getBy, AriaRole.Textbox, name), name, description)
    {
        public async Task EnterTextAsync(string text)
        {
            await _locator.FillAsync(text);
        }

        public async Task<string> GetTextAsync()
        {
            return await _locator.InnerTextAsync();
        }
    }
}
