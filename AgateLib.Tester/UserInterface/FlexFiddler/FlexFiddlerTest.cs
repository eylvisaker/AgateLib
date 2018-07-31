using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.FlexFiddler
{
    public class FlexFiddlerTest : UITest
    {
        public override string Name => "Flex Fiddler";

        protected override Workspace InitializeWorkspace()
        {
            Scene.Theme = "FF";

            return new Workspace("default",
                new FlexFiddlerApp(new FlexFiddlerAppProps
                {
                    OnCancel = e => OnExit?.Invoke()
                }));
        }
    }
}
