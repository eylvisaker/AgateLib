using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui.Layout
{
    public abstract class BoxLayoutBase : ILayoutPerformer 
    {
        bool doingLayout = false;
        List<int> sizes = new List<int>();
        Container container;



        public Size RecalcMinSize(Container container)
        {
            this.container = container;
            return RecalcMinSizeInternal();
        }

        protected Size RecalcMinSizeBox(bool horizontal)
        {
            _horizontal = horizontal;

            Size minSize = new Size();
            int totalSize = 0;

            foreach (Widget child in container.Children)
            {
                child.RecalcMinSize();

                minSize.Width = Math.Max(minSize.Width, child.MinSize.Width);
                minSize.Height = Math.Max(minSize.Height, child.MinSize.Height);

                totalSize += GetSize(child.MinSize);
            }

            return SetSize(minSize, totalSize);
        }

        public void DoLayout(Container container)
        {
            if (doingLayout)
                return;

            this.container = container;

            try
            {
                doingLayout = true;

                DoLayoutInternal();
            }
            finally
            {
                doingLayout = false;
            }
        }

        protected abstract void DoLayoutInternal();
        protected abstract Size RecalcMinSizeInternal();

        bool _horizontal;
        protected void DoBoxLayout(bool horizontal)
        {
            _horizontal = horizontal;
            
            int containerSize = GetContainerSize();
            int totalMinSize = 0;
            int expandCount = 0;
            int shrinkCount = 0;
            sizes.Clear();

            foreach (Widget child in container.Children.VisibleItems)
            {
                int minSize = GetMinSize(child);

                switch (child.LayoutExpand)
                {
                    case LayoutExpand.Default:
                        totalMinSize += minSize;
                        break;

                    case LayoutExpand.ExpandToMax:
                        expandCount++;
                        break;

                    case LayoutExpand.ShrinkToMin:
                        totalMinSize += minSize;
                        shrinkCount++;
                        break;
                }
            }

            int extraSpace = containerSize - totalMinSize;
            if (extraSpace < 0)
                extraSpace = 0;

            if (expandCount == 0)
            {
                if (shrinkCount < container.Children.VisibleItems.Count() 
                    && extraSpace > 0)
                {
                    int expandSize = extraSpace / 
                        (container.Children.VisibleItems.Count() - shrinkCount);

                    ShareSpace(expandSize);
                }
                else
                    ShareSpaceEqually();

            }
            else
            {
                int expandSize = extraSpace / expandCount;

                SetMinSizes(expandSize);
            }
        }

        private void SetMinSizes(int expandSize)
        {
            int loc = 0;

            foreach (Widget child in container.Children.VisibleItems)
            {
                int size;

                switch (child.LayoutExpand)
                {
                    case LayoutExpand.Default:
                    case LayoutExpand.ShrinkToMin:
                        size = GetMinSize(child);
                        break;

                    case LayoutExpand.ExpandToMax:
                        size = expandSize;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                SetLocation(child, loc);
                SetSize(child, size);

                loc += size;
            }
        }

        private void ShareSpace(int extraSpace)
        {
            int loc = 0;
            int containerSize = GetContainerSize();

            foreach (Widget child in container.Children.VisibleItems)
            {
                int size = GetMinSize(child);
                
                if (child.LayoutExpand != LayoutExpand.ShrinkToMin)
                    size += extraSpace;

                SetLocation(child, loc);
                SetSize(child, size);

                loc += size;
            }

        }
        private void ShareSpaceEqually()
        {
            int loc = 0;
            int containerSize = GetContainerSize();

            foreach (Widget child in container.Children.VisibleItems)
            {
                int size = containerSize / container.Children.Count;

                if (child.LayoutExpand == LayoutExpand.ShrinkToMin)
                    size = GetMinSize(child);

                SetLocation(child, loc);
                SetSize(child, size);

                loc += size;
            }
             
        }
        int GetSize(Size size)
        {
            if (_horizontal)
                return size.Width;
            else
                return size.Height;
        }
        int GetMinSize(Widget widget)
        {
            return GetSize(widget.MinSize);
        }

        private Size SetSize(Size size, int value)
        {
            if (_horizontal)
                return new Size(value, size.Height);
            else
                return new Size(size.Width, value);
        }
        void SetSize(Widget widget, int value)
        {
            if (_horizontal)
                widget.Size = new Size(value, container.ClientArea.Height);
            else
                widget.Size = new Size(container.ClientArea.Width, value);
        }
        void SetLocation(Widget widget, int value)
        {
            if (_horizontal)
                widget.Location = new Point(value, 0);
            else
                widget.Location = new Point(0, value);
        }

        int GetContainerSize()
        {
            if (_horizontal)
                return container.ClientArea.Width;
            else
                return container.ClientArea.Height;
        }



        public virtual bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
        {
            return false;

        }

    }
}
