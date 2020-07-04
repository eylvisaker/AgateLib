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

//#define __DEBUG_RENDER

using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface.Styling;
using Microsoft.Xna.Framework;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AgateLib.UserInterface
{
    public class VisualTree
    {
        private readonly IAnimationFactory animationFactory;
        private readonly Logger log;
        private IRenderElement root;
        private IRenderElement focus;

        private int debugFlag;

        public VisualTree(UserInterfaceConfig config, IAnimationFactory animationFactory)
        {
            Config = config;
            this.animationFactory = animationFactory;
            log = LogManager.GetCurrentClassLogger();
        }

        public IDisplaySystem DisplaySystem { get; set; }

        public IRenderElement Focus => focus;

        public IRenderElement TreeRoot => root;

        public IStyleConfigurator Style { get; set; }

        public UserInterfaceAppContext AppContext { get; internal set; }

        public UserInterfaceConfig Config { get; internal set; }

        public float VisualScaling => Config.VisualScaling;

        public void Render(IRenderable rootRenderable)
        {
            DebugMsg($"Rendering from {rootRenderable}", setDebugFlag: 1);

            rootRenderable.AppContext = AppContext;

            var newRoot = rootRenderable.FinalizeRendering(_ => Render(rootRenderable));
            bool anyUpdates = false;

            Reconcile(ref root, newRoot, ref anyUpdates);

            // The order of operations of the below is critical.
            // 1. Set the appcontext, display system, and parent of every item in the tree.
            // 2. OnReconciliationCompleted should be called first, so
            //    that any containers that cache lists of children can update.
            // 3. Check to make sure the focus element is still valid and reset it if not. Must be done
            //    after OnReconciliationCompleted because of element caching.
            // 4. Apply styles. This must be done after the focus is updated so the focus element has the correct 
            //    styling.
            // 5. Call element.Style.Update and animationFactory.Configure on the element. Do this after applying 
            //    styles so animations work correctly.
            Walk((element, parent) =>
            {
                element.AppContext = AppContext;

                element.Display.System = DisplaySystem;
                element.Display.Parent = parent?.Display;

                return true;
            });

            if (anyUpdates)
            {
                Walk((element, parent) =>
                {
                    element.OnReconciliationCompleted();
                    return true;
                });
            }

            if (Focus?.Parent == null)
            {
                if (root.CanHaveFocus)
                {
                    SetFocus(root);
                }
                else
                {
                    SetFocus(null);
                }
            }

            Style.Apply(root, DisplaySystem.Theme);

            Walk((element, parent) =>
            {
                element.Style.Update(VisualScaling);

                animationFactory.Configure(element.Display);

                return true;
            });
        }

        private void Reconcile(ref IRenderElement oldNode, IRenderElement newNode, ref bool anyUpdates)
        {
            if (oldNode == null && newNode == null)
            {
                return;
            }

            if (oldNode == null)
            {
                Replace(ref oldNode, newNode);
                anyUpdates = true;
                return;
            }
            if (newNode == null)
            {
                Unmount(oldNode);
                oldNode = null;
                anyUpdates = true;
                return;
            }

            if (oldNode.GetType() != newNode.GetType())
            {
                Replace(ref oldNode, newNode);
                anyUpdates = true;
            }
            else if (!oldNode.Props.PropertiesEqual(newNode.Props))
            {
                oldNode.SetProps(newNode.Props);

                anyUpdates = true;

                if (newNode.Ref != null)
                {
                    newNode.Ref.Current = oldNode;
                }
            }
            else if (newNode.Ref != null)
            {
                newNode.Ref.Current = oldNode;
            }

            if ((oldNode.Children?.Count ?? 0) == 0 && (newNode.Children?.Count ?? 0) == 0)
            {
                return;
            }

            if (oldNode.ChildReconciliationMode == ChildReconciliationMode.Self)
            {
                oldNode.ReconcileChildren(newNode);
            }
            else
            {
                bool childrenUpdated = newNode.Children.Count != oldNode.Children.Count;

                var children = new List<IRenderElement>();

                newNode.AppContext = AppContext;

                // Reorder the children in the old node so that they match the ones in the new node.
                for (int newIndex = 0; newIndex < newNode.Children.Count; newIndex++)
                {
                    var newItem = newNode.Children[newIndex];
                    var old = FindMatch(newItem, oldNode.Children, newIndex);

                    if (old != null)
                    {
                        Reconcile(ref old, newItem, ref anyUpdates);
                        children.Add(old);
                    }
                    else
                    {
                        anyUpdates = true;
                        children.Add(newItem);
                    }

                    childrenUpdated |= anyUpdates;
                }

                oldNode.Children.Clear();

                foreach (var child in children)
                {
                    oldNode.Children.Add(child);
                }

                if (childrenUpdated)
                {
                    oldNode.OnChildrenUpdated();
                }
            }
        }

        /// <summary>
        /// Matches an old node to one in the new tree by the key parameters. 
        /// Or returns the item at the defaultIndex position in the children array.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="children"></param>
        /// <param name="defaultIndex"></param>
        private IRenderElement FindMatch(IRenderElement oldKid, IList<IRenderElement> newKids, int newIndex)
        {
            if (!string.IsNullOrWhiteSpace(oldKid.Props.Key))
            {
                return newKids.FirstOrDefault(x => x.Props.Key == oldKid.Props.Key);
            }

            if (newIndex >= newKids.Count)
            {
                return null;
            }

            return newKids[newIndex];
        }

        /// <summary>
        /// Matches an old node to one in the new tree by the key parameters. 
        /// Or returns the item at the defaultIndex position in the children array.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="children"></param>
        /// <param name="defaultIndex"></param>
        private IRenderElement FindMatch_Unused(IList<IRenderElement> oldKids, int oldIndex, IList<IRenderElement> newKids)
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
            {
                return;
            }

            newNode.AppContext = AppContext;
            newNode.OnDidMount();
        }

        private void Unmount(IRenderElement oldNode)
        {
            if (oldNode == null)
            {
                return;
            }

            oldNode.OnWillUnmount();
        }

        public bool SetFocus(IRenderElement newFocus)
        {
            if (focus == newFocus) return false;
            if (!newFocus.CanHaveFocus) return false;

            IRenderElement oldFocus = focus;

            focus = newFocus;

            bool accepted = focus?.OnFocus() ?? true;

            if (!accepted)
            {
                focus = oldFocus;
                return false;
            }

            newFocus?.Parent?.Display.ScrollTo(focus.Display);
            newFocus?.Display.PseudoClasses.Add("focus");

            oldFocus?.Display.PseudoClasses.Remove("focus");
            oldFocus?.OnBlur();

            return true;
        }

        public void Update(IUserInterfaceRenderContext renderContext)
        {
            DebugMsg("Updating all widgets", ifDebugFlagAtLeast: 1);

            Walk(element =>
            {
                element.Update(renderContext);
                element.Style.Update(VisualScaling);

                return true;
            });

            DoLayout(renderContext);

            Walk(element =>
            {
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
            Walk(root, null, (node, parent) => action(node));
        }

        /// <summary>
        /// Walks the entire visual tree using a depth first approach.
        /// </summary>
        /// <param name="action">An action to execute on each render element. The first argument
        /// is the render element, the second is its parent. Parent will be null for the root element.
        /// Return true to continue the walk, false to exit.</param>
        public void Walk(Func<IRenderElement, IRenderElement, bool> action)
        {
            Walk(root, null, action);
        }

        private bool Walk(IRenderElement node, IRenderElement parent, Func<IRenderElement, IRenderElement, bool> action)
        {
            bool cont = action(node, parent);

            if (!cont)
            {
                return false;
            }

            if (node.Children == null)
            {
                return true;
            }

            foreach (var child in node.Children)
            {
                cont = Walk(child, node, action);

                if (!cont)
                {
                    return false;
                }
            }

            return true;
        }

        public void Draw(IUserInterfaceRenderContext renderContext)
        {
            DebugMsg("Drawing all widgets", ifDebugFlagAtLeast: 1, setDebugFlag: 0);

            renderContext.DrawChild(Config.ScreenArea, TreeRoot);
        }

        public void DoLayout(IUserInterfaceLayoutContext layoutContext)
        {
            Rectangle area = Config.ScreenArea;

            TreeRoot.Display.MarginRect = area;
            TreeRoot.DoLayout(layoutContext, LayoutMath.MarginToContent(area, TreeRoot).Size);

            CheckForOverflow();
        }

        private void CheckForOverflow()
        {
            Walk(element =>
            {
                HasOverflow overflow = HasOverflow.None;

                if (element.Children != null)
                {
                    if (element.Children.Any(x =>
                        x.Display.MarginRect.Right > element.Display.ContentRect.Width))
                    {
                        overflow |= HasOverflow.X;
                    }
                    if (element.Children.Any(x =>
                        x.Display.MarginRect.Bottom > element.Display.ContentRect.Height))
                    {
                        overflow |= HasOverflow.Y;
                    }
                }

                element.Display.HasOverflow = overflow;

                return true;
            });
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

                matcher = e => e.Name == id;
            }

            Walk(e =>
            {
                if (matcher(e))
                {
                    results.Add(e);
                }

                return true;
            });

            return results;

        }


        [Conditional("__DEBUG_RENDER")]
        private void DebugMsg(string message, int ifDebugFlagAtLeast = 0, int? setDebugFlag = null)
        {
            if (debugFlag >= ifDebugFlagAtLeast)
            {
                log.Debug(message);
            }

            if (setDebugFlag != null)
            {
                debugFlag = setDebugFlag.Value;
            }
        }
    }
}
