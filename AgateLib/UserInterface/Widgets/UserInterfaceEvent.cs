using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class UserInterfaceEvent
    {
        public UserInterfaceEvent() { }
        public UserInterfaceEvent(IRenderElement sender)
        {
            Reset(sender);
        }

        public void Reset(IRenderElement sender)
        {
            Sender = sender;
        }

        public IRenderElement Sender { get; private set; }

        public IDisplaySystem System => Sender.Display.System;
    }

    public class UserInterfaceEvent<T> : UserInterfaceEvent
    {
        public UserInterfaceEvent<T> Reset(IRenderElement sender, T data)
        {
            base.Reset(sender);
            Data = data;

            return this;
        }

        public UserInterfaceEvent<T> Reset(UserInterfaceEvent baseEvent, T data)
        {
            base.Reset(baseEvent.Sender);
            Data = data;

            return this;
        }

        public T Data { get; private set; }
    }

    public delegate void UserInterfaceEventHandler(UserInterfaceEvent e);
    public delegate void UserInterfaceEventHandler<T>(UserInterfaceEvent<T> e);
}
