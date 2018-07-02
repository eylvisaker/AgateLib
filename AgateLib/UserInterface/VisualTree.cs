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
using AgateLib.Display;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using AgateLib.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace AgateLib.UserInterface
{
    public class VisualTree
    {
        IRenderElement root;
        IRenderElement focus;

        public void Render(IRenderElement newRoot)
        {
            root = Reconcile(root, newRoot);
            root.Display.ParentFont = DisplaySystem.Fonts.Default;

            Style.Apply(root, DefaultTheme);

            Walk(element =>
            {
                element.Display.System = DisplaySystem;
                return true;
            });
        }

        private IRenderElement Reconcile(IRenderElement oldNode, IRenderElement newNode)
        {
            if (oldNode == null)
                return newNode;

            return oldNode;

            //var array = enumerable.ToList();

            //var removed = tree.Where(x => !array.Contains(x));
            //var added = array.Where(x => !tree.Contains(x));

            //foreach (var item in removed)
            //    item.Display.Animation.State = AnimationState.TransitionOut;
            //foreach (var item in added)
            //    item.Display.Animation.State = AnimationState.TransitionIn;

            //// TODO: get the ordering right, so that elements in tree are ordered the same as in enumerable.
            //tree.AddRange(added);

            //tree.RemoveAll(x => x.Display.Animation.State == AnimationState.Dead);
        }
        
        public IDisplaySystem DisplaySystem { get; set; }

        public IRenderElement Focus
        {
            get => focus;
            set
            {
                focus?.Display.PseudoClasses.Remove("focus");
                focus?.OnBlur();

                focus = value;

                focus?.OnFocus();
                focus?.Display.PseudoClasses.Add("focus");
            }
        }

        public IRenderElement TreeRoot => root;

        public IStyleConfigurator Style { get; set; }

        public string DefaultTheme { get; set; }

        public void Update(IWidgetRenderContext renderContext)
        {
            Walk(element =>
            {
                element.Update(renderContext);
                element.Style.Update();
                renderContext.UpdateAnimation(element);
                return true;
            });
        }

        /// <summary>
        /// Walks the entire visual tree, using a depth first approach.
        /// </summary>
        /// <param name="action">An action to execute on each render element. Return true to continue walking, false to exit.</param>
        public void Walk(Func<IRenderElement, bool> action)
        {
            Walk(root, action);
        }

        private bool Walk(IRenderElement node, Func<IRenderElement, bool> action)
        {
            bool cont = action(node);

            if (!cont)
                return false;

            if (node.Children == null)
                return true;

            foreach (var item in node.Children)
            {
                item.Display.ParentFont = node.Style.Font;

                cont = Walk(item, action);

                if (!cont)
                    return false;
            }

            return true;
        }

        public void Draw(IWidgetRenderContext renderContext, Rectangle area)
        {
            TreeRoot.Display.MarginRect = area;
            TreeRoot.DoLayout(renderContext, TreeRoot.Display.Region.MarginToContentOffset.Contract(area).Size);

            renderContext.DrawChild(area, TreeRoot);
        }
    }
}
