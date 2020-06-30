using AgateLib.UserInterface;
using System.Collections.Generic;

namespace AgateLib.Demo.UserInterface.RadioButtons
{
    public class RadioButtonTest : UITest
    {
        public override string Name => "Radio Buttons";

        protected override IRenderable CreateUIRoot()
        {
            return new RadioButtonApp(new RadioButtonAppProps
            {
                Items = new List<string> { "Foo", "Bar", "Gra", "San", "Zen" },
                OnCancel = e => ExitTest(),
            });
        }
    }
}
