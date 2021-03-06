﻿using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.Scrolling
{
    public class ScrollingTest : UIDemo
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
