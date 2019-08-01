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
    public class TextBox : RenderElement<TextBoxProps>
    {
        private string text;
        private int insertionPoint;

        private const float flashTime = 0.5f;
        private float flashCycle = 0;

        public TextBox(TextBoxProps props) : base(props)
        {
            text = props.Text;
        }

        public override string StyleTypeId => "textbox";

        public override bool CanHaveFocus => Props.Enabled;

        public override Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize)
        {
            return new Size(80, Style.Font.FontHeight);
        }

        public override void Update(IUserInterfaceRenderContext renderContext)
        {
            flashCycle += (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            if (flashCycle > flashTime)
                flashCycle %= flashTime;

            base.Update(renderContext);
        }

        public override void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size)
        {

        }

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
        {
            Vector2 screenDest = clientArea.Location.ToVector2();

            renderContext.Canvas.DrawText(Style.Font, screenDest, text);

            Vector2 insertionPointLoc = new Vector2(Style.Font.MeasureString(text.Substring(0, insertionPoint)).Width, 0);

            if (flashCycle < flashTime / 2)
            {
                //renderContext.DrawLine(insertionPointLoc, insertionPointLoc + Vector2.UnitY * Style.Font.FontHeight);
            }
        }
    }

    public class TextBoxProps : RenderElementProps
    {
        public TextBoxProps()
        {
            DefaultStyle = new InlineElementStyle
            {
                Border = new BorderStyle
                {
                    Top = new BorderSideStyle { Width = 1, Color = Color.White, },
                    Left = new BorderSideStyle { Width = 1, Color = Color.White, },
                    Right = new BorderSideStyle { Width = 1, Color = Color.White, },
                    Bottom = new BorderSideStyle { Width = 1, Color = Color.White, },
                },
                Padding = new LayoutBox { Top = 2, Bottom = 2, Left = 4, Right = 4 },
                Margin = LayoutBox.SameAllAround(2),
            };
        }

        public UserInterfaceEventHandler<string> OnChange { get; set; }
        public string Text { get; set; }
    }
}
