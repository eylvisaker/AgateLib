using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.TextAlignment
{
    public class DoubleRadioMenusTest : UITest
    {
        public override string Name => "Text Alignment";

        protected override IRenderable CreateUIRoot()
        {
            return new TextAlignmentApp(new TextAlignmentAppProps
            {
                OnCancel = e => ExitTest(),
            });
        }
    }
}
