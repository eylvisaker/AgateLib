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

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class WidgetEventArgs : EventArgs, IWidgetEventArgsInitialize
    {
        public static WidgetEventArgs ButtonDown(MenuInputButton button)
        {
            var result = new WidgetEventArgs();
            result.InitializeButtonDown(button);
            return result;
        }

        public static WidgetEventArgs ButtonUp(MenuInputButton button)
        {
            var result = new WidgetEventArgs();
            result.InitializeButtonUp(button);
            return result;
        }

        public WidgetEventType EventType { get; private set; }

        public MenuInputButton Button { get; private set; }

        public bool Handled { get; set; }

        public Point Location { get; internal set; }

        private void InitializeButtonDown(MenuInputButton button)
        {
            EventType = WidgetEventType.ButtonDown;
            Button = button;
            Handled = false;
        }

        private void InitializeButtonUp(MenuInputButton button)
        {
            EventType = WidgetEventType.ButtonUp;
            Button = button;
            Handled = false;
        }

        void IWidgetEventArgsInitialize.InitializeButtonDown(MenuInputButton button)
        {
            InitializeButtonDown(button);
        }

        void IWidgetEventArgsInitialize.InitializeButtonUp(MenuInputButton button)
        {
            InitializeButtonUp(button);
        }

        public override string ToString()
        {
            return $"{Button} {EventType} (Handled: {Handled})";
        }

        internal WidgetEventArgs Initialize(WidgetEventType eventType)
        {
            Location = Point.Zero;
            EventType = eventType;

            return this;
        }
    }

    public enum WidgetEventType
    {
        ButtonDown,
        ButtonUp,

        FocusGained,
        FocusLost,
        DrawComplete,
    }

    internal interface IWidgetEventArgsInitialize
    {
        void InitializeButtonDown(MenuInputButton button);

        void InitializeButtonUp(MenuInputButton button);
    }
}
