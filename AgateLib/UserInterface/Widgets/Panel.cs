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
using System.Linq;
using System.Text;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Layout;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface
{
    /// <summary>
    /// Widget that contains other widgets in a layout.
    /// </summary>
    public class Panel : Widget<PanelProps>
    {
        public Panel(PanelProps props) : base(props)
        {
        }

        public override IRenderable Render() => new FlexBox(new FlexBoxProps
        {
            AllowNavigate = Props.AllowNavigate,
            StyleTypeId = "panel",
            Children = Props.Children.ToList(),
            OnCancel = Props.OnCancel,
            Enabled = Props.Enabled,
        }.CopyFromWidgetProps(Props));
    }

    public class PanelProps : WidgetProps
    {
        public bool AllowNavigate { get; set; } = true;

        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public UserInterfaceEventHandler OnCancel { get; set; }

        public bool Enabled { get; set; } = true;
    }
}
