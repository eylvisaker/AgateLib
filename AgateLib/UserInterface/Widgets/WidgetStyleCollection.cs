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

namespace AgateLib.UserInterface.Widgets
{
    /// <summary>
    /// The collection of styles available to a widget. This allows 
    /// a widget to change styling based on its state, for example, 
    /// the selected item in a menu item will gain the "selected" state.
    /// </summary>
    public class WidgetStyleCollection
    {
        const string defaultKey = "default";

        Dictionary<string, WidgetStyle> styles 
            = new Dictionary<string, WidgetStyle>(StringComparer.OrdinalIgnoreCase);

        WidgetStyle activeStyle;

        public WidgetStyleCollection()
        {
            styles.Add(defaultKey, new WidgetStyle());
            activeStyle = styles[defaultKey];
        }

        public event EventHandler ActiveStyleChanged;

        public WidgetStyle DefaultStyle => styles[defaultKey];

        public WidgetStyle ActiveStyle
        {
            get => activeStyle;
            private set
            {
                if (activeStyle == value)
                    return;

                activeStyle = value;

                ActiveStyleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or creates a style for the specified widget state.
        /// </summary>
        /// <param name="state">The state of the widget where this
        /// style should be active.</param>
        /// <returns></returns>
        public WidgetStyle GetOrCreate(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                return DefaultStyle;

            if (styles.TryGetValue(state, out WidgetStyle result) == false)
            {
                result = new WidgetStyle();
                styles[state] = result;
            }

            return result;
        }

        /// <summary>
        /// Sets the active style based on the current state of the widget.
        /// </summary>
        /// <param name="state"></param>
        public void SetActiveStyle(string state)
        {
            if (styles.TryGetValue(state, out WidgetStyle result))
            {
                SetState(result);
            }
            else
            {
                SetState(DefaultStyle);
            }
        }

        private void SetState(WidgetStyle widgetStyle)
        {
            ActiveStyle = widgetStyle;
        }
    }
}
