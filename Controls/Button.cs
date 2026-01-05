using Microsoft.Playwright;

namespace Controls
{
    public class Button(IPage page, Control.GetBy getBy, string name, string description) 
        : Control(GetLocator(page, getBy, AriaRole.Button, name), name, description)
    {
    }
}
