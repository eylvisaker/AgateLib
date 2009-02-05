using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.Diagnostics;

namespace AgateLib.Gui
{
    public abstract class Container : Widget
    {
        WidgetList mChildren;
        Rectangle mClientArea;
        ILayoutPerformer mLayout = new Layout.VerticalBox();
        bool mLayoutSuspended;

        public Container()
        {
            mChildren = new WidgetList(this);
            mChildren.ListUpdated += new EventHandler(mChildren_ListUpdated);
        }

        public Rectangle ClientArea { get { return mClientArea; } }
        public Size ClientSize
        {
            get { return mClientArea.Size; }
            set
            {
                Size = Root.ThemeEngine.RequestClientAreaSize(this, value);
            }
        }
        public virtual ILayoutPerformer Layout
        {
            get { return mLayout; }
            set
            {
                mLayout = value;
                RedoLayout();
            }
        }

        public override Point PointToScreen(Point p)
        {
            return base.PointToScreen(
                new Point(p.X + ClientArea.X, p.Y + ClientArea.Y));
        }
        public override Point PointToClient(Point p)
        {
            Point retval = base.PointToClient(p);

            retval.X -= ClientArea.X;
            retval.Y -= ClientArea.Y;

            return retval;
        }
        protected override void OnResize()
        {
            if (Root == null)
                return;

            mClientArea = Root.ThemeEngine.GetClientArea(this);
            DoLayoutPrivate();

            base.OnResize();
        }
        protected override void OnParentChanged()
        {
            if (Parent == null)
                return;

            OnResize();
            base.OnParentChanged();
        }

        public void SuspendLayout()
        {
            mLayoutSuspended = true;

            foreach (Container child in mChildren)
            {
                child.SuspendLayout();
            }
        }
        public void ResumeLayout()
        {
            mLayoutSuspended = false;

            foreach (Widget child in mChildren)
            {
                if (child is Container == false)
                    continue;

                ((Container)child).ResumeLayout();
            }

            RedoLayout();
        }
        protected internal override void RecalcMinSize()
        {
            if (mLayoutSuspended)
                return;

            MinSize = Layout.RecalcMinSize(this);
        }

        void mChildren_ListUpdated(object sender, EventArgs e)
        {
            DoLayoutPrivate();
        }

        public WidgetList Children
        {
            get { return mChildren; }
        }

        protected internal override void UpdateGui()
        {
            for (int i = 0; i < Children.Count; i++)
                Children[i].UpdateGui();

            base.UpdateGui();
        }

        void DoLayoutPrivate()
        {
            RedoLayout();
            Invalidate();
        }

        public virtual void RedoLayout()
        {
            if (mLayoutSuspended)
                return;

            RecalcMinSize();
            Layout.DoLayout(this);
        }
        protected override void DoDraw()
        {
            base.DoDraw();

            foreach (var child in Children.VisibleItems)
                child.Draw();
        }

        protected internal override bool AcceptFocusOnMouseDown
        {
            get { return false; }
        }

        protected internal override bool AcceptInputKey(KeyCode keyCode)
        {
            return Layout.AcceptInputKey(keyCode);
        }

        //Widget FindMouseInControl(Point screenMousePoint)
        //{
        //    foreach (Widget child in mChildren)
        //    {
        //        if (child.ContainsScreenPoint(screenMousePoint) == true)
        //            return child;
        //    }

        //    return null;
        //}
    }
}