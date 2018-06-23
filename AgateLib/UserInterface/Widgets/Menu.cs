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

namespace AgateLib.UserInterface.Widgets
{
    public class Menu : Window
    {
        /// <summary>
        /// Out of order! Fuck, even in the future nothing works.
        /// </summary>
        private bool cancelButton = false;
        private CancelEventArgs cancelEventArgs = new CancelEventArgs();

        public Menu(string name = "") : base(name) { }

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

        [Obsolete("Set indicator at desktop level.", true)]
        public IMenuIndicatorRenderer Indicator { get; set; }

        public override void ProcessEvent(WidgetEventArgs args)
        {
            base.ProcessEvent(args);
        }

        protected override void OnFocusGained()
        {
            base.OnFocusGained();
        }

        /// <summary>
        /// Adds an item to the menu.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        public void Add(string text, Action action)
        {
            var item = new ContentMenuItem { Text = text, Name = text };
            item.PressAccept += (sender, e) => action();

            Layout.Add(item);
        }

        /// <summary>
        /// Adds an item to the menu.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        public void Add(string name, string text, Action action)
        {
            var item = new ContentMenuItem { Text = text, Name = name };
            item.PressAccept += (sender, e) => action();

            Layout.Add(item);
        }

        protected override void OnButtonUp(MenuInputButton button)
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

            if (Layout.Focus == null && Layout.Count > 0)
                Layout.Focus = Layout.First();
        }

        protected override void OnButtonDown(MenuInputButton button)
        {
            switch (button)
            {
                case MenuInputButton.Cancel:
                    cancelButton = true;
                    break;
            }

            base.OnButtonDown(button);
        }

        public override void Draw(
            IWidgetRenderContext renderContext,
            Point clientDest)
        {
            foreach (var child in Layout.Items)
            {
                if (child == Layout.Focus)
                {
                    child.Display.Styles.SetActiveStyle("selected");
                }
                else
                {
                    child.Display.Styles.SetActiveStyle("");
                }
            }

            renderContext.DrawChildren(clientDest, Layout.Items);
        }
    }
}
