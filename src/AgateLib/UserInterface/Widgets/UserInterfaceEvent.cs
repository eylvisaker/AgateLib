//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;

namespace AgateLib.UserInterface
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
            Arg1 = data;

            return this;
        }

        public UserInterfaceEvent<T> Reset(UserInterfaceEvent baseEvent, T data)
        {
            base.Reset(baseEvent.Sender);
            Arg1 = data;

            return this;
        }

        public T Arg1 { get; private set; }

        [Obsolete("Use Arg1 instead.")]
        public T Data => Arg1;

    }

    public class UserInterfaceEvent<T1, T2> : UserInterfaceEvent
    {
        public UserInterfaceEvent<T1, T2> Reset(IRenderElement sender, T1 data1, T2 data2)
        {
            base.Reset(sender);

            Arg1 = data1;
            Arg2 = data2;

            return this;
        }

        public UserInterfaceEvent<T1, T2> Reset(UserInterfaceEvent baseEvent, T1 data1, T2 data2)
        {
            base.Reset(baseEvent.Sender);

            Arg1 = data1;
            Arg2 = data2;

            return this;
        }

        public T1 Arg1 { get; private set; }

        public T2 Arg2 { get; private set; }

        [Obsolete("Use Arg1 instead.")]
        public T1 Data1 => Arg1;

        [Obsolete("Use Arg1 instead.")]
        public T2 Data2 => Arg2;
    }

    public class UserInterfaceEvent<T1, T2, T3> : UserInterfaceEvent
    {
        public UserInterfaceEvent<T1, T2, T3> Reset(IRenderElement sender, T1 data1, T2 data2, T3 data3)
        {
            base.Reset(sender);

            Arg1 = data1;
            Arg2 = data2;
            Arg3 = data3;

            return this;
        }

        public UserInterfaceEvent<T1, T2, T3> Reset(UserInterfaceEvent baseEvent, T1 data1, T2 data2, T3 data3)
        {
            base.Reset(baseEvent.Sender);

            Arg1 = data1;
            Arg2 = data2;
            Arg3 = data3;

            return this;
        }

        public T1 Arg1 { get; private set; }

        public T2 Arg2 { get; private set; }

        public T3 Arg3 { get; private set; }

        [Obsolete("Use Arg1 instead.")]
        public T1 Data1 => Arg1;

        [Obsolete("Use Arg1 instead.")]
        public T2 Data2 => Arg2;

        [Obsolete("Use Arg1 instead.")]
        public T3 Data3 => Arg3;
    }

    public delegate void UserInterfaceEventHandler(UserInterfaceEvent e);
    public delegate void UserInterfaceEventHandler<T>(UserInterfaceEvent<T> e);
    public delegate void UserInterfaceEventHandler<T1, T2>(UserInterfaceEvent<T1, T2> e);
    public delegate void UserInterfaceEventHandler<T1, T2, T3>(UserInterfaceEvent<T1, T2, T3> e);

    public delegate void UserInterfaceEventHandler2<T>(UserInterfaceEvent e, T arg);

}
