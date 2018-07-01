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

using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Transitions;
using System.ComponentModel;
using System.Linq;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Widgets
{
    public class MenuProps : WidgetProps
    {
        public IList<MenuItem> Children { get; set; } = new List<MenuItem>();

        public IWidget InitialFocus { get; set; }
    }

    public class MenuState : WidgetState
    {

    }

    public class Menu : Widget<MenuProps, MenuState>
    {
        /// <summary>
        /// Out of order! Fuck, even in the future nothing works.
        /// </summary>
        private bool cancelButton = false;
        private CancelEventArgs cancelEventArgs = new CancelEventArgs();

        public Menu(MenuProps props) : base(props)
        {
            SetState(new MenuState());

        }

        public event Action Exit;
        public event Action<CancelEventArgs> Cancel;

        /// <summary>
        /// Exits the menu.
        /// </summary>
        protected virtual void OnExit()
        {
            Exit?.Invoke();
        }

        /// <summary>
        /// Called when the user presses cancel. If eventArgs.Cancel is set to true, this
        /// will cancel the cancellation and prevent OnExit from being called.
        /// </summary>
        /// <param name="eventArgs"></param>
        protected virtual void OnCancel(CancelEventArgs eventArgs)
        {
            Cancel?.Invoke(eventArgs);
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

        protected void OnButtonUp(MenuInputButton button)
        {
            switch (button)
            {
                case MenuInputButton.Cancel:
                    if (cancelButton)
                    {
                        cancelEventArgs.Cancel = false;
                        OnCancel(cancelEventArgs);

                        if (!cancelEventArgs.Cancel)
                        {
                            OnExit();
                        }

                        cancelButton = false;
                    }

                    break;
            }
        }

        public override void Update(IWidgetRenderContext renderContext)
        {
            base.Update(renderContext);

            throw new NotImplementedException();
            //if (Layout.Focus == null && Layout.Count > 0)
            //    Layout.Focus = Layout.First();
        }

        public override IRenderable Render()
        {
            return new MenuElement(new MenuElementProps
            {
                StyleId = Props.Name,
                StyleClass = "menu",
                Children = Props.Children.ToList<IRenderable>()
            });
        }

        protected void OnButtonDown(MenuInputButton button)
        {
            switch (button)
            {
                case MenuInputButton.Cancel:
                    cancelButton = true;
                    break;
            }
        }
    }

    public class MenuElement : RenderElement<MenuElementProps>
    {
        private FlexBox child;
        private int selectedIndex;
        private ButtonPress<MenuInputButton> buttonPress = new ButtonPress<MenuInputButton>();

        public MenuElement(MenuElementProps props) : base(props)
        {
            child = new FlexBox(new FlexBoxProps
            {
                Children = props.Children
            });

            Children = new[] { child };

            SelectedMenuItem.Display.PseudoClasses.Add("selected");

            buttonPress.Press += OnButtonPress;
        }

        IRenderElement SelectedMenuItem => child.Children.Skip(selectedIndex).First();

        public override bool CanHaveFocus => true;

       
        public override Size CalcIdealContentSize(IWidgetRenderContext renderContext, Size maxSize)
        {
            return child.CalcIdealContentSize(renderContext, maxSize);
        }

        public override void Draw(IWidgetRenderContext renderContext, Rectangle clientArea)
        {
            renderContext.DrawChild(clientArea, child);
        }

        List<MenuInputButton> buttonsDown = new List<MenuInputButton>();

        public override void OnInputEvent(InputEventArgs e)
        {
            if (e.EventType == WidgetEventType.ButtonDown)
            {
                buttonPress.ButtonDown(e.Button);

                var newIndex = selectedIndex;

                if (e.Button == MenuInputButton.Down)
                    newIndex++;
                if (e.Button == MenuInputButton.Up)
                    newIndex--;
                
                if (newIndex < 0)
                    newIndex = 0;
                if (newIndex >= Props.Children.Count)
                    newIndex = Props.Children.Count - 1;

                if (newIndex != selectedIndex)
                {
                    SelectedMenuItem.Display.PseudoClasses.Remove("selected");
                    selectedIndex = newIndex;
                    SelectedMenuItem.Display.PseudoClasses.Add("selected");
                }
            }
            else if (e.EventType == WidgetEventType.ButtonUp)
            {
                buttonPress.ButtonUp(e.Button);

            }

            base.OnInputEvent(e);
        }

        private void OnButtonPress(MenuInputButton btn)
        {
            if (btn == MenuInputButton.Accept)
            {
                SelectedMenuItem?.OnAccept();
            }
        }

        public override void OnBlur()
        {
            base.OnBlur();

            buttonPress.Clear();
        }
    }

    public class MenuElementProps : RenderElementProps
    {
        public IList<IRenderable> Children { get; set; } = new List<IRenderable>();
    }
}
