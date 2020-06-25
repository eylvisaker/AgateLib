using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface
{
    public abstract class BinElement<T> : RenderElement<T> where T : BinElementProps
    {
        private IRenderElement child;
        private List<IRenderElement> children = new List<IRenderElement>();

        protected BinElement(T props) : base(props)
        {
            ReceiveBinProps();
        }

        public IRenderElement Child => child;

        public override IList<IRenderElement> Children
        {
            get => children;
            protected set
            {
                if (value.Count > 0)
                {
                    child = value[0];
                    children.Clear();
                    children.Add(child);
                }
                else
                {
                    child = null;
                    children.Clear();
                }
            }
        }

        protected override void OnReceivedAppContext()
        {
            base.OnReceivedAppContext();

            ReceiveBinProps();
        }

        public override Size CalcMinContentSize(int? widthConstraint, int? heightConstraint)
            => child.CalcMinMarginSize(widthConstraint, heightConstraint);

        public override Size CalcIdealContentSize(IUserInterfaceLayoutContext layoutContext, Size maxSize)
        {
            if (child == null)
                return new Size(0, 0);

            return child.CalcIdealMarginSize(layoutContext, maxSize);
        }

        public override void DoLayout(IUserInterfaceLayoutContext layoutContext, Size size)
            => DoLayoutForSingleChild(layoutContext, size, child);

        public override void Draw(IUserInterfaceRenderContext renderContext, Rectangle clientArea)
            => renderContext.DrawChild(clientArea, child);

        protected override void OnReceiveProps()
        {
            base.OnReceiveProps();

            ReceiveBinProps();
        }

        private void ReceiveBinProps()
        {
            if (AppContext == null)
                return;

            child = FinalizeRendering(Props.Child);
            children.Clear();
            children.Add(child);
        }
    }

    public abstract class BinElementProps : RenderElementProps
    {
        public IRenderable Child { get; set; }
    }
}
