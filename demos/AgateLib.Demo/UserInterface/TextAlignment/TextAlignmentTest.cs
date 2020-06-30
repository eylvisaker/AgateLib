﻿using AgateLib.UserInterface;

namespace AgateLib.Demo.UserInterface.TextAlignment
{
    public class TextAlignmentTest : UITest
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