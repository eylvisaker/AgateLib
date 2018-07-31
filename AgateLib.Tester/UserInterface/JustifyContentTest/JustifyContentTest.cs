using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.JustifyContentTest
{
    public class JustifyContentTester : UITest
    {
        public override string Name => "Justify Content";

        protected override Workspace InitializeWorkspace() => new Workspace("default",
            new JustifyContentApp(new JustifyContentAppProps
            {
                OnCancel = e => OnExit?.Invoke()
            }));
    }
}
