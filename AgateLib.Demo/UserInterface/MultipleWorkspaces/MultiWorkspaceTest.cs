﻿using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.MultipleWorkspaces
{
    public class MultiWorkspaceTest : UITest
    {
        public override string Name => "Multiple Workspaces";

        protected override IRenderable CreateUIRoot()
            => new MultiWorkspaceApp(new MultiWorkspaceAppProps());
    }
}
