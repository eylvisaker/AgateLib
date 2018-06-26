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

namespace AgateLib.UserInterface.Widgets
{
    public class WindowProps : WidgetProps
    {
        public IList<IWidget> Children { get; set; }

        public IWidget Focus { get; set; }
    }

    /// <summary>
    /// Widget that contains other widgets in a layout.
    /// Does not handle navigation between widgets, that must be handled manually.
    /// </summary>
    public class Window : Widget<WindowProps, WidgetState>
    {
        private IWidgetLayout layout = new SingleColumnLayout();
        private List<IWidget> children = new List<IWidget>();

        public Window(WindowProps props) : base(props)
        {
        }

        /// <summary>
        /// Gets or sets the layout type of the menu.
        /// Setting this property allows for switching between single column
        /// or single row layouts. For any custom layout type, set the Layout property.
        /// </summary>
        public LayoutType LayoutType
        {
            get
            {
                var type = Layout.GetType();

                if (type == typeof(SingleColumnLayout)) return LayoutType.SingleColumn;
                if (type == typeof(SingleRowLayout)) return LayoutType.SingleRow;

                return LayoutType.Custom;
            }
            set
            {
                if (value == LayoutType)
                    return;

                if (value == LayoutType.Custom)
                    return;

                IWidgetLayout newLayout = null;

                switch (value)
                {
                    case LayoutType.SingleColumn:
                        newLayout = new SingleColumnLayout();
                        break;

                    case LayoutType.SingleRow:
                        newLayout = new SingleRowLayout();
                        break;

                    default:
                        throw new ArgumentException("Cannot understand layout type.");
                }

                layout = newLayout;
            }
        }

        /// <summary>
        /// Adds a widget to the window layout.
        /// </summary>
        /// <param name="widget"></param>
        [Obsolete("Pass children in props")]
        public void Add(IWidget widget)
        {
            children.Add(widget);
        }

        /// <summary>
        /// Gets or sets the IWidgetLayout object that handles the actual layout
        /// of the menu items. Cannot be null.
        /// </summary>
        [Obsolete]
        public IWidgetLayout Layout
        {
            get => layout;
            set => layout = value ?? throw new ArgumentNullException(nameof(Layout));
        }

        [Obsolete]
        public virtual void ProcessEvent(WidgetEventArgs args)
        {
            Layout?.InputEvent(args);
        }

        public virtual void Update(IWidgetRenderContext renderContext)
        {
            foreach (var item in children)
                item.Update(renderContext);
        }

        public override IRenderElement Render()
        {
            layout.SetChildren(children.Select(c => c.Render()));
            return layout;
        }
    }


    public enum LayoutType
    {
        SingleColumn,
        SingleRow,

        Custom,
    }
}
