using Microsoft.Playwright;

namespace Controls
{
    public class TextBox(IPage page, Control.GetBy getBy, string name) : Control(GetLocator(page, getBy, AriaRole.Textbox, name))
    {
        public async Task EnterTextAsync(string text)
        {
            await Locator.FillAsync(text);
        }

        public async Task<string> GetTextAsync()
        {
            return await Locator.InnerTextAsync();
        }
    }
}
