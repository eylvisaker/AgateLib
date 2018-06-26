using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface
{
    public class VisualTree
    {
        List<IRenderElement> tree = new List<IRenderElement>();

        public void Render(IEnumerable<IRenderElement> enumerable)
        {
            var array = enumerable.ToList();

            var removed = tree.Where(x => !array.Contains(x));
            var added = array.Where(x => !tree.Contains(x));

            foreach (var item in removed)
                item.Display.Animation.State = AnimationState.TransitionOut;
            foreach (var item in added)
                item.Display.Animation.State = AnimationState.TransitionIn;

            // TODO: get the ordering right, so that elements in tree are ordered the same as in enumerable.
            tree.AddRange(added);

            tree.RemoveAll(x => x.Display.Animation.State == AnimationState.Dead);
        }

        public IEnumerable<IRenderElement> Items => tree;

        public void Update(IWidgetRenderContext renderContext)
        {
            foreach (var item in tree)
                item.Update(renderContext);
        }
    }
}
