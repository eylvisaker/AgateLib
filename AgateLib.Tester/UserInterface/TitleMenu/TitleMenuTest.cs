using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.TitleMenu
{
    public class TitleMenuTest : UITest
    {
        public override string Name => "Example Title Menu";

        protected override Workspace InitializeWorkspace()
        {
            return new Workspace("default", new TitleMenuApp(new TitleMenuAppProps { OnCancel = e => OnExit?.Invoke() }));
        }
    }
}
