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
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface.Widgets
{
    public class Menu : Widget<MenuProps, WidgetState>
    {
        public Menu(MenuProps props) : base(props)
        {
        }

        /// <summary>
        /// Adds an item to the menu.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        [Obsolete("Pass children in props", true)]
        public void Add(string name, string text, Action action)
        {
            throw new NotSupportedException();
        }

        public override IRenderable Render()
        {
            return new MenuElement(new MenuElementProps
            {
                StyleId = Props.Name,
                StyleClass = "menu",
                Cancel = Props.Cancel,
                Enabled = Props.Enabled,
                InitialSelectionIndex = Props.InitialSelectionIndex,
                Children = Props.MenuItems.ToList<IRenderable>()
            });
        }
    }

    public class MenuProps : WidgetProps
    {
        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        /// <summary>
        /// The zero-based index of the first item that should be selected.
        /// </summary>
        public int InitialSelectionIndex { get; set; }

        /// <summary>
        /// Callback to execute if the user presses cancel (usually the B button).
        /// </summary>
        public Action Cancel { get; set; }

        public bool Enabled { get; set; } = true;
    }

    public class MenuElement : RenderElement<MenuElementProps>
    {
        private FlexBox child;
        private int selectedIndex = -1;
        private ButtonPress<MenuInputButton> buttonPress = new ButtonPress<MenuInputButton>();

        public MenuElement(MenuElementProps props) : base(props)
        {
            child = new FlexBox(new FlexBoxProps
            {
                DefaultStyle = props.DefaultStyle ?? new InlineElementStyle
                {
                    Flex = new FlexStyle {
                        AlignItems = AlignItems.Stretch
                    }
                },
                Style = props.Style,
                StyleClass = props.StyleClass,
                StyleId = props.StyleId,
                StyleTypeId = string.IsNullOrWhiteSpace(props.StyleTypeId)
                            ? "menu" : props.StyleTypeId,
                Children = props.Children,
                InitialFocusIndex = props.InitialSelectionIndex
            });

            Children = new[] { child };

            selectedIndex = Props.InitialSelectionIndex;

            buttonPress.Press += OnButtonPress;
        }
        
        IRenderElement SelectedMenuItem
            => selectedIndex < child.Children.Count()
               ? child.Children.Skip(selectedIndex).First() : null;

        public int SelectedIndex => child.FocusIndex;

        public IEnumerable<MenuItemElement> MenuItems => child.Children.OfType<MenuItemElement>();

        public override bool CanHaveFocus => child.CanHaveFocus && Props.Enabled;

        public override void DoLayout(IWidgetRenderContext renderContext, Size size)
            => DoLayoutForSingleChild(renderContext, size, child);

        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            return child.CalcIdealMarginSize(renderContext, maxSize);
        }

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            child.Display.MarginRect = new Rectangle(Point.Zero, clientArea.Size);
            renderContext.DrawChild(clientArea, child);
        }

        public override void OnFocus()
        {
            base.OnFocus();
            Display.System.SetFocus(child);
        }

        public override void OnBlur()
        {
            base.OnBlur();

            buttonPress.Clear();
        }
        
        public override void OnChildNavigate(IRenderElement child, MenuInputButton button)
        {
            if (button == MenuInputButton.Cancel && Props.Cancel != null)
            {
                Props.Cancel();
                return;
            }

            base.OnChildNavigate(child, button);
        }

        private void OnButtonPress(MenuInputButton btn)
        {
            switch (btn)
            {
                case MenuInputButton.Accept:
                    SelectedMenuItem?.OnAccept();
                    break;

                case MenuInputButton.Cancel:
                    Props.Cancel?.Invoke();
                    break;
            }
        }
    }

    public class MenuElementProps : RenderElementProps
    {
        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();

        public Action Cancel { get; set; }

        public string StyleTypeId { get; set; }

        public int InitialSelectionIndex { get; set; }
    }
}
