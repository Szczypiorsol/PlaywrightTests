using Microsoft.Playwright;

namespace Controls
{
    public class Button(IPage page, Control.GetBy getBy, string name) : Control(GetLocator(page, getBy, name, AriaRole.Button))
    {
    }
}
