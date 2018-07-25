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

        public UserInterfaceEvent Reset(IRenderElement sender)
        {
            Sender = sender;
            return this;
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

    public class UserInterfaceEvent<T1, T2> : UserInterfaceEvent
    {
        public UserInterfaceEvent<T1, T2> Reset(IRenderElement sender, T1 data1, T2 data2)
        {
            base.Reset(sender);

            Data1 = data1;
            Data2 = data2;

            return this;
        }

        public UserInterfaceEvent<T1, T2> Reset(UserInterfaceEvent baseEvent, T1 data1, T2 data2)
        {
            base.Reset(baseEvent.Sender);

            Data1 = data1;
            Data2 = data2;

            return this;
        }

        public T1 Data1 { get; private set; }

        public T2 Data2 { get; private set; }
    }

    public class UserInterfaceEvent<T1, T2, T3> : UserInterfaceEvent
    {
        public UserInterfaceEvent<T1, T2, T3> Reset(IRenderElement sender, T1 data1, T2 data2, T3 data3)
        {
            base.Reset(sender);

            Data1 = data1;
            Data2 = data2;
            Data3 = data3;

            return this;
        }

        public UserInterfaceEvent<T1, T2, T3> Reset(UserInterfaceEvent baseEvent, T1 data1, T2 data2, T3 data3)
        {
            base.Reset(baseEvent.Sender);

            Data1 = data1;
            Data2 = data2;
            Data3 = data3;

            return this;
        }

        public T1 Data1 { get; private set; }

        public T2 Data2 { get; private set; }

        public T3 Data3 { get; private set; }
    }

    public delegate void UserInterfaceEventHandler(UserInterfaceEvent e);
    public delegate void UserInterfaceEventHandler<T>(UserInterfaceEvent<T> e);
    public delegate void UserInterfaceEventHandler<T1, T2>(UserInterfaceEvent<T1, T2> e);
    public delegate void UserInterfaceEventHandler<T1, T2, T3>(UserInterfaceEvent<T1, T2, T3> e);
}
