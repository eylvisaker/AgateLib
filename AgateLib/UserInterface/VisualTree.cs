using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Display;
using AgateLib.UserInterface.Rendering;
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
                element.Style.Update();
                renderContext.UpdateAnimation(element);
                return true;
            });

            root.Update(renderContext);
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

        internal void Draw(IWidgetRenderContext renderContext, Rectangle area)
        {
            TreeRoot.Display.MarginRect = area;

            renderContext.DrawChild(area, TreeRoot);
        }
    }
}
