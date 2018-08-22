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
using System.Collections.Generic;

namespace AgateLib.UserInterface
{
    public class App : Widget<AppProps>
    {
        public App(AppProps props) : base(props)
        {
        }

        public override IRenderable Render() => new FlexBox(new FlexBoxProps
        {
            Name = Props.Name,
            Theme = Props.Theme,
            Style = Props.Style,
            StyleClass = Props.StyleClass,
            DefaultStyle = Props.DefaultStyle ?? new InlineElementStyle
            {
                Flex = new FlexStyle
                {
                    Direction = FlexDirection.Row,
                    AlignItems = AlignItems.Center,
                    JustifyContent = JustifyContent.SpaceEvenly,
                },
                Padding = new LayoutBox(100, 50, 100, 50),
            },
            Visible = Props.Visible,
            StyleTypeId = "workspace",
            OnCancel = Props.OnCancel,
            Children = Props.Children,
        });
    }

    public class AppProps : WidgetProps
    {
        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public UserInterfaceEventHandler OnCancel { get; set; }
    }
}
