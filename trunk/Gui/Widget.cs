using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.Diagnostics;

namespace AgateLib.Gui
{
    public abstract class Widget
    {
        Rectangle mRegion;
        Container mParent;
        string mText;
        LayoutExpand mLayoutExpand;
        bool mAutoCalcMinSize = true;
        bool mAutoCalcMaxSize = true;
        Size mMinSize, mMaxSize;
        bool mVisible = true;
        bool mEnabled = true;

        bool suspendLayout = false;

        public Widget()
        {
            mText = string.Empty;
            mMaxSize = new Size(9000, 9000);
        }

        public override string ToString()
        {
            return base.ToString() + ": " + Name;
        }
        protected GuiRoot Root
        {
            get
            {
                if (Parent == null && this is GuiRoot)
                    return (GuiRoot)this;
                else if (Parent == null)
                    // this is for an object which has not been added to 
                    // a container yet.
                    return null;
                else
                    return Parent.Root;
            }
        }

        public bool CanHaveFocus { get; protected set; }
        public bool HasFocus
        {
            get
            {
                if (Root == null)
                    return false;
                else
                    return Root.FocusControl == this;
            }
        }
        public virtual string Text
        {
            get { return mText; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Text may not be null.");

                mText = value;
                RecalcSizeRange();
            }
        }

        public string Name { get; set; }
        public string TooltipText { get; set; }
        public LayoutExpand LayoutExpand
        {
            get { return mLayoutExpand; }
            set
            {
                mLayoutExpand = value;

                if (Parent != null)
                    Parent.RedoLayout();
            }
        }

        public Container Parent
        {
            get { return mParent; }
            internal set
            {
                mParent = value;
                OnParentChanged();
            }
        }

        protected virtual void OnParentChanged()
        {
            RecalcSizeRange();
        }
        protected internal virtual void RecalcSizeRange()
        {
            if (Root == null)
                return;
            if (suspendLayout)
                return;

            try
            {
                suspendLayout = true;

                if (AutoCalcMinSize)
                    MinSize = Root.ThemeEngine.CalcMinSize(this);

                if (AutoCalcMaxSize)
                    MaxSize = Root.ThemeEngine.CalcMaxSize(this);
            }
            finally
            {
                suspendLayout = false;

                Parent.RedoLayout();
            }
        }

        public bool Visible
        {
            get { return mVisible; }
            set
            {
                if (value == mVisible)
                    return;

                mVisible = value;

                if (Parent != null)
                    Parent.RedoLayout();
            }
        }
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }

        public Point Location
        {
            get { return mRegion.Location; }
            set { mRegion.Location = value; }
        }
        public Size Size
        {
            get { return mRegion.Size; }
            set
            {
                if (mRegion.Size.Width == value.Width &&
                    mRegion.Size.Height == value.Height)
                    return;

                if (value.Width < MinSize.Width ||
                    value.Height < MinSize.Height)
                    throw new AgateGuiException("Cannot make widget smaller than its MinSize.");
                if (value.Width > MaxSize.Width ||
                    value.Height > MaxSize.Height)
                    throw new AgateGuiException("Cannot make widget larget than its MaxSize.");

                mRegion.Size = value;
                OnResizePrivate();
            }
        }
        protected internal Rectangle Region
        {
            get { return mRegion; }
        }

        public int Width
        {
            get { return Size.Width; }
        }
        public int Height
        {
            get { return Size.Height; }
        }

        public Size MinSize
        {
            get { return mMinSize; }
            set
            {
                mMinSize = value;

                if (this == Root) return;
                if (suspendLayout == false && Root != null)
                    Parent.RedoLayout();
            }
        }
        public Size MaxSize
        {
            get { return mMaxSize; }
            set
            {
                mMaxSize = value;

                if (this == Root) return;
                if (suspendLayout == false && Root != null)
                    Parent.RedoLayout();
            }
        }
        public bool AutoCalcMinSize
        {
            get { return mAutoCalcMinSize; }
            set
            {
                mAutoCalcMinSize = value;

                if (value == true)
                    RecalcSizeRange();

                Parent.RedoLayout();
            }
        }
        public bool AutoCalcMaxSize
        {
            get { return mAutoCalcMinSize; }
            set
            {
                mAutoCalcMaxSize = value;

                if (value == true)
                    RecalcSizeRange();

                Parent.RedoLayout();
            }
        }

        bool IsDirty { get { return mDirtyRects.Count != 0; } }
        List<Rectangle> mDirtyRects = new List<Rectangle>();

        public virtual void Invalidate()
        {
            mDirtyRects.Clear();
            mDirtyRects.Add(Region);

            if (Parent != null)
                Parent.Invalidate(mRegion);
        }
        public void Invalidate(Rectangle region)
        {
            for (int i = 0; i < mDirtyRects.Count; i++)
            {
                if (Rectangle.Intersect(mDirtyRects[i], region) == region)
                    return;
            }

            mDirtyRects.Add(region);

            if (Parent != null)
                Parent.Invalidate(mRegion);
        }

        private void OnResizePrivate()
        {
            mDirtyRects.Add(mRegion);
            OnResize();
        }
        protected virtual void OnResize()
        {
            if (Resize != null)
                Resize(this, EventArgs.Empty);

        }

        protected internal virtual bool AcceptFocusOnMouseDown
        {
            get { return true; }
        }

        public event EventHandler Resize;

        public void Draw()
        {
            DoDraw();
        }
        /// <summary>
        /// Override to do actual drawing commands.  BeginFrame will already be set.
        /// </summary>
        protected virtual void DoDraw()
        {
            Root.ThemeEngine.DrawWidget(this);
        }

        protected internal virtual void UpdateGui()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">A point in client coordinates</param>
        /// <returns></returns>
        public virtual Point PointToScreen(Point p)
        {
            if (Parent == null && this is GuiRoot)
                return p;
            else if (Parent == null)
                throw new InvalidOperationException();

            return Parent.PointToScreen(
                new Point(
                    p.X + Location.X,
                    p.Y + Location.Y));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">A point in window coordinates</param>
        /// <returns></returns>
        public virtual Point PointToClient(Point p)
        {
            if (Parent == null && this is GuiRoot)
                return p;
            else if (Parent == null)
                throw new InvalidOperationException();

            Point parentPoint = Parent.PointToClient(p);

            return new Point(parentPoint.X - Location.X, parentPoint.Y - Location.Y);
        }

        internal bool HitTest(Point screenLocation)
        {
            return Root.ThemeEngine.HitTest(this, screenLocation);
        }

        internal Point ScreenLocation
        {
            get
            {
                if (this is GuiRoot)
                    return Location;

                return Parent.PointToScreen(Location);
            }
        }

        internal int ThemeMargin
        {
            get { return Root.ThemeEngine.ThemeMargin(this); }
        }

        public bool MouseIn { get; protected set; }
        protected internal virtual void SendMouseDown(InputEventArgs e) { OnMouseDown(e); }
        protected internal virtual void SendMouseUp(InputEventArgs e) { OnMouseUp(e); }
        protected internal virtual void SendMouseMove(InputEventArgs e) { OnMouseMove(e); }
        protected internal virtual void SendMouseDoubleClick(InputEventArgs e) { OnMouseDoubleClick(e); }
        protected internal virtual void SendKeyDown(InputEventArgs e) { OnKeyDown(e); }
        protected internal virtual void SendKeyUp(InputEventArgs e) { OnKeyUp(e); }
        /// <summary>
        /// Sends the MouseEnter event to this control.
        /// If overriding this, be sure to set MouseIn to true, and provide
        /// some logic for calling OnMouseEnter on the control.
        /// </summary>
        protected internal virtual void SendMouseEnter() { MouseIn = true; OnMouseEnter(); }
        /// <summary>
        /// Sends the MouseLeave event to this control.
        /// If overriding this, be sure to set MouseIn to false, and provide
        /// some logic for calling OnMouseLeave on the control.
        /// 
        /// </summary>
        protected internal virtual void SendMouseLeave() { MouseIn = false; OnMouseLeave(); }

        protected internal virtual void OnMouseDown(InputEventArgs e) { }
        protected internal virtual void OnMouseUp(InputEventArgs e) { }
        protected internal virtual void OnMouseMove(InputEventArgs e) { }
        protected internal virtual void OnMouseDoubleClick(InputEventArgs e) { }
        protected internal virtual void OnKeyDown(InputEventArgs e) { }
        protected internal virtual void OnKeyUp(InputEventArgs e) { }
        protected internal virtual void OnMouseEnter() { }
        protected internal virtual void OnMouseLeave() { }

        public bool ContainsScreenPoint(Point screenMousePoint)
        {
            return new Rectangle(ScreenLocation, Size).Contains(screenMousePoint);
        }

        protected internal virtual void OnGainFocus()
        {
        }
        protected internal virtual void OnLoseFocus()
        {
        }

        protected internal virtual bool AcceptInputKey(KeyCode keyCode)
        {
            return false;
        }
    }
}