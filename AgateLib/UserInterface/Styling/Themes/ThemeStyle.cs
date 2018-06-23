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

using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Styling.Themes.Model;

namespace AgateLib.UserInterface.Styling.Themes
{
    public class ThemeStyle
    {
        #region --- Matching Widgets ---

        internal IWidgetSelector Selector { get; set; }

        /// <summary>
        /// Matches widgets with a pattern.
        /// "type" matches all widgets of that type.
        /// "*" matches all widgets.
        /// "type.*" matches all widgets that are children of widgets which match type.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Indicates a pattern should only apply for a particular state of a widget.
        /// Right now, only "selected" is used for menu items.
        /// </summary>
        public string WidgetState { get; set; }

        #endregion

        #region --- Properties to apply ---

        public ThemeWidgetAnimation Animation { get; set; }

        public ThemeWidgetBackground Background { get; set; }

        public ThemeWidgetBorder Border { get; set; }

        public FontStyleProperties Font { get; set; }

        public LayoutBox? Padding { get; set; }

        public LayoutBox? Margin { get; set; }

        public ThemeWidgetSize Size { get; set; }

        #endregion

        #region --- Debug Info ---

        public override string ToString()
        {
            var b = new PropertyDebugStringBuilder();

            b.Add(nameof(Selector), Selector);
            b.Add(nameof(Font), Font);
            b.Add(nameof(Background), Background);
            b.Add(nameof(Padding), Padding);
            b.Add(nameof(Margin), Margin);

            return b.ToString();
        }

        #endregion
    }
}
