
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.RadioButtons
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
