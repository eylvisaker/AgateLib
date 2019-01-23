using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.Scrolling
{
    public class ScrollingTest : UITest
    {
        public override string Name => "Scrolling";

        protected override IRenderable CreateUIRoot()
        {
            return new ScrollingApp(new ScrollingAppProps
            {
                OnCancel = e => ExitTest(),
            });
        }
    }
}
