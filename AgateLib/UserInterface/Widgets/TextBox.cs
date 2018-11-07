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
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface
{
    public class TextBox : Widget<TextBoxProps, TextBoxState>
    {
        public TextBox(TextBoxProps props) : base(props)
        {
        }

        public override IRenderable Render()
        {
            return new TextBoxElement(new TextBoxElementProps
            {
                Value = Props.Value,
                OnChange = Props.OnChange,
            });
        }
    }

    public class TextBoxProps : WidgetProps
    {
        public string Value { get; set; }

        public UserInterfaceEventHandler<string> OnChange { get; set; }
    }

    public class TextBoxState
    {
    }


    public class TextBoxElement : RenderElement<TextBoxElementProps>
    {
        public TextBoxElement(TextBoxElementProps props) : base(props)
        {
        }

        public override string StyleTypeId => "textbox";

        public override bool CanHaveFocus => Props.Enabled;

        public override Size CalcIdealContentSize(IUserInterfaceRenderContext renderContext, Size maxSize)
        {
            return new Size(80, Style.Font.FontHeight);
        }

        public override void DoLayout(IUserInterfaceRenderContext renderContext, Size size)
        {

        }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            Style.Font.DrawText(renderContext.SpriteBatch, clientArea.Location.ToVector2(), Props.Value);
        }
    }

    public class TextBoxElementProps : RenderElementProps
    {
        public string Value { get; set; }

        public UserInterfaceEventHandler<string> OnChange { get; set; }
    }
}
