using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class ElementEvent
    {
        public ElementEvent(IRenderElement sender)
        {
            Sender = sender;
        }

        public IRenderElement Sender { get; }

        public IDisplaySystem System => Sender.Display.System;
    }

    public delegate void RenderElementEventHandler(ElementEvent e);
}
