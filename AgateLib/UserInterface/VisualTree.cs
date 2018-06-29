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

        public void Render(IRenderElement newRoot)
        {
            root = CompareNodes(root, newRoot);

            Style.Apply(root, DefaultTheme);
        }

        private IRenderElement CompareNodes(IRenderElement oldNode, IRenderElement newNode)
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

        public IRenderElement TreeRoot => root;

        public IStyleConfigurator Style { get; set; }

        public string DefaultTheme { get; set; }

        public void Update(IWidgetRenderContext renderContext)
        {
            Walk(x =>
            {
                x.Display.Fonts = renderContext.Fonts;
                x.Style.Update();
                renderContext.UpdateAnimation(x);
            });

            root.Update(renderContext);
        }

        private void Walk(Action<IRenderElement> action)
        {
            Walk(root, action);
        }

        private void Walk(IRenderElement node, Action<IRenderElement> action)
        {
            action(node);

            if (node.Children == null)
                return;

            foreach (var item in node.Children)
                Walk(item, action);
        }
    }
}
