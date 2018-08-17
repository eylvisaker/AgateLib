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
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.InputMap
{
    [Obsolete("I don't think this is needed anymore? Double check after keyboard and general joystick input is handled.")]
    public class ButtonPress<T> where T : struct
    {
        HashSet<T> buttonsDown = new HashSet<T>();

        public void ButtonDown(T button)
        {
            buttonsDown.Add(button);
        }

        public void ButtonUp(T button)
        {
            if (buttonsDown.Contains(button))
            {
                buttonsDown.Remove(button);
                Press?.Invoke(button);
            }
        }

        public bool IsButtonDown(T button) => buttonsDown.Contains(button);
        public bool IsButtonUp(T button) => !buttonsDown.Contains(button);

        public event Action<T> Press;

        public void Clear()
        {
            buttonsDown.Clear();
        }
    }

    //public static class ButtonPressExtensions
    //{
    //    public static void HandleInputEvent(this ButtonPress<UserInterfaceAction> buttonPress, InputEventArgs input)
    //    {
    //        if (input.EventType == WidgetEventType.ButtonDown)
    //            buttonPress.ButtonDown(input.Button);
    //        if (input.EventType == WidgetEventType.ButtonUp)
    //            buttonPress.ButtonUp(input.Button);
    //    }
    //}
}
