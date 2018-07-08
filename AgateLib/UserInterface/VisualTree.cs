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

        public void Render(IRenderable rootRenderable)
        {
            var newRoot = rootRenderable.Finalize(_ => Render(rootRenderable));

            Reconcile(ref root, newRoot);
            root.Display.ParentFont = DisplaySystem.Fonts.Default;

            Style.Apply(root, DefaultTheme);

            Walk(element =>
            {
                element.Display.System = DisplaySystem;
                return true;
            });
        }

        private void Reconcile(ref IRenderElement oldNode, IRenderElement newNode)
        {
            if (oldNode == null)
            {
                Replace(ref oldNode, newNode);
                return;
            }

            if (newNode == null)
            {
                Unmount(oldNode);
                oldNode = null;
                return;
            }

            if (oldNode.GetType() != newNode.GetType())
            {
                Replace(ref oldNode, newNode);
            }

            if (!oldNode.Props.PropertiesEqual(newNode.Props))
            {
                oldNode.SetProps(newNode.Props);
            }

            if ((oldNode.Children?.Count ?? 0) == 0 && (newNode.Children?.Count ?? 0) == 0)
            {
                return;
            }

            bool childrenUpdated = false;

            for (int i = 0; i < oldNode.Children.Count || i < newNode.Children.Count; i++)
            {
                if (i < oldNode.Children.Count)
                {
                    var old = oldNode.Children[i];
                    var match = FindMatch(oldNode.Children, i, newNode.Children);

                    Reconcile(ref old, match);

                    if (!ReferenceEquals(old, oldNode.Children[i]))
                    {
                        oldNode.Children[i] = old;
                        childrenUpdated = true;
                    }
                }
                else
                {
                    oldNode.Children.Add(newNode.Children[i]);
                    childrenUpdated = true;
                }
            }

            for (int i = oldNode.Children.Count - 1; i > 0; i--)
            {
                if (oldNode.Children[i] == null)
                    oldNode.Children.RemoveAt(i);
            }

            if (childrenUpdated)
            {
                oldNode.OnChildrenUpdated();
            }
        }

        /// <summary>
        /// Matches an old node to one in the new tree by the key parameters. 
        /// Or returns the item at the defaultIndex position in the children array.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="children"></param>
        /// <param name="defaultIndex"></param>
        private IRenderElement FindMatch(IList<IRenderElement> oldKids, int oldIndex, IList<IRenderElement> newKids)
        {
            var old = oldKids[oldIndex];

            if (!string.IsNullOrWhiteSpace(old.Props.Key))
            {
                return newKids.FirstOrDefault(x => x.Props.Key == old.Props.Key);
            }

            int blankCount = oldKids.Take(oldIndex).Count(x => string.IsNullOrWhiteSpace(x.Props.Key));

            return newKids.Where(x => string.IsNullOrWhiteSpace(x.Props.Key)).Skip(blankCount).FirstOrDefault();
        }

        private void Replace(ref IRenderElement oldNode, IRenderElement newNode)
        {
            Unmount(oldNode);
            oldNode = newNode;
            Mount(newNode);
        }

        private void Mount(IRenderElement newNode)
        {
            if (newNode == null)
                return;

            newNode.OnDidMount();
        }

        private void Unmount(IRenderElement oldNode)
        {
            if (oldNode == null)
                return;

            oldNode.OnWillUnmount();
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
            DoLayout(renderContext, area);

            renderContext.DrawChild(area, TreeRoot);
        }

        public void DoLayout(IWidgetRenderContext renderContext, Rectangle area)
        {
            TreeRoot.Display.MarginRect = area;
            TreeRoot.DoLayout(renderContext, TreeRoot.Display.Region.MarginToContentOffset.Contract(area).Size);
        }

        /// <summary>
        /// Finds a render element in the tree.
        /// </summary>
        /// <param name="selector">A string of form "#styleId" </param>
        /// <returns></returns>
        public IEnumerable<IRenderElement> Find(string selector)
        {
            // TODO: Refactor the CSS matching logic so it can be used from here.
            List<IRenderElement> results = new List<IRenderElement>();
            Func<IRenderElement, bool> matcher = null;

            if (selector.StartsWith("#"))
            {
                var id = selector.Substring(1);

                matcher = e => e.StyleId == id;
            }

            Walk(e =>
            {
                if (matcher(e))
                    results.Add(e);
                return true;
            });

            return results;

        }
    }
}
