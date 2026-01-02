using Microsoft.Playwright;
using static Controls.Control;

namespace Controls
{
    public class TextBox(IPage page, GetBy getBy, string name) : Control(GetLocator(page, getBy, AriaRole.Textbox, name), name)
    {
        public async Task EnterTextAsync(string text)
        {
            await _locator.FillAsync(text);
        }

        public async Task AssertTextAsync(string ExpectedText)
        {
            await Assertions.Expect(_locator).ToHaveTextAsync(ExpectedText);
        }
    }
}
