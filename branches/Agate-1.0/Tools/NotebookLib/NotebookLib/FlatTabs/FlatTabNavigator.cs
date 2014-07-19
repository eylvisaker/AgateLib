using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib.FlatTabs
{
    [ToolboxItem(false)]
    class FlatTabNavigator : Control, INavigator
    {
        Notebook owner;
        FlatTabProperties navProperties;
        int mSelIndex = -1;
        double tabTextHeightRatio = 1.75;

        #region --- Tab clicking Fields ---

        bool mDraggingPage = false;
        NotebookPage mPageClickIn;

        bool mClickCloseBox = false;
        int mOverCloseBox = -1;
        int mClickCloseBoxIndex;

        Font mBoldFont;

        #endregion

        List<NotebookPage> visiblePages = new List<NotebookPage>();

        TabRenderInfo[] tabs;

        const int imageMargin = 2;
        const int tabMargin = 2;
        const int pageHeight = 3;

        public FlatTabNavigator(Notebook owner)
        {
            this.owner = owner;

            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public int SelectedIndex
        {
            get
            {
                return mSelIndex;
            }
            set
            {
                mSelIndex = value;

                Invalidate();
                owner.OnSelectedPageChanged();
            }
        }
        public void RefreshAllPages()
        {
            visiblePages.Clear();
            visiblePages.AddRange(owner.VisiblePages());

            tabs = null;

            UpdateTabLocations();

            Invalidate();
        }

        public NavigatorProperties CreateProperties()
        {
            navProperties = new FlatTabProperties(owner, this);
            return navProperties;
        }

        public Size NavMinSize
        {
            get
            {
                int minSize = TabHeight + pageHeight + 1;

                return new Size(minSize, minSize);
            }
        }
        public Size NavMaxSize
        {
            get
            {
                return NavMinSize;
            }
        }
       
        private int TabIndexFromPoint(Point mouseClickPoint)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].Bounds.Contains(mouseClickPoint))
                {
                    return owner.NotebookPages.IndexOf(visiblePages[i]);
                }
            }

            return -1;
        }
        
        public void OnUpdateLayout()
        {

        }

        bool ParentDesign
        {
            get { return Parent != null && Parent.Site != null && Parent.Site.DesignMode; }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (mOverCloseBox != -1)
            {
                if (ParentDesign == false)
                {
                    mClickCloseBox = true;
                    mClickCloseBoxIndex = mOverCloseBox;
                }
            }
            else
            {
                int clickIndex = TabIndexFromPoint(e.Location);

                if (0 <= clickIndex && clickIndex < visiblePages.Count)
                {
                    SelectedIndex = clickIndex;

                    if (navProperties.AllowTabReorder)
                    {
                        mDraggingPage = true;
                        mPageClickIn = visiblePages[clickIndex];
                    }
                }
            }

            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {

            if (mDraggingPage)
            {
                int newVisibleIndex = 0;
                int pageDragIndex = visiblePages.IndexOf(mPageClickIn);

                // search through the visible pages to 
                // find which pages this one should be between
                for (int i = 0; i < tabs.Length; i++)
                {
                    if (e.X > tabs[i].Bounds.Left + tabs[i].Bounds.Width / 2)
                    {
                        newVisibleIndex = i + 1;

                        if (i == pageDragIndex)
                            newVisibleIndex--;
                    }
                    else
                        break;
                }

                // find which page it should go before in the NotebookPage list.
                NotebookPage pageBefore = null;

                if (newVisibleIndex < visiblePages.Count)
                    pageBefore = visiblePages[newVisibleIndex];

                if (pageDragIndex != newVisibleIndex)
                {
                    // these indices are in NotebookPages, not VisiblePages.
                    int newIndex = owner.NotebookPages.IndexOf(pageBefore);
                    int oldIndex = owner.NotebookPages.IndexOf(mPageClickIn);

                    // first, renumber all the pages
                    // copy them to a local list because the list will be reordered
                    // everytime the Order property is changed.
                    List<NotebookPage> pages = new List<NotebookPage>();
                    pages.AddRange(owner.NotebookPages);

                    for (int i = 0; i < pages.Count; i++)
                    {
                        pages[i].Order = 10 * i;
                    }

                    if (pageBefore != null)
                    {
                        mPageClickIn.Order = pageBefore.Order - 5;
                    }
                    else
                    {
                        // move to end of tabs
                        mPageClickIn.Order = pages[pages.Count - 1].Order + 10;
                    }
                    owner.SelectedPage = mPageClickIn;

                    Refresh();
                }
            }
            else
            {
                int newCloseBox = -1;

                for (int i = 0; i < tabs.Length; i++)
                {
                    if (tabs[i].CloseBox.Contains(e.Location))
                    {
                        newCloseBox = i;
                        break;
                    }
                }

                if (newCloseBox != mOverCloseBox)
                {
                    if (mOverCloseBox != -1)                        Invalidate(tabs[mOverCloseBox].CloseBox);
                    if (newCloseBox != -1)                    Invalidate(tabs[newCloseBox].CloseBox);

                    mOverCloseBox = newCloseBox;
                }
                
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {

            if (mClickCloseBox && ParentDesign == false)
            {
                if (mClickCloseBoxIndex == mOverCloseBox)
                {
                    owner.OnCloseTab(visiblePages[mClickCloseBoxIndex]);

                    mOverCloseBox = -1;
                    mClickCloseBoxIndex = -1;
                }
            }

            mClickCloseBox = false;
            mDraggingPage = false;

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            
            UpdateTabLocations();

            Brush selBrush = new SolidBrush(owner.Navigator.PageBackColor);
            Pen pathPen = SystemPens.ControlDark;

            Rectangle pageExtendRect = new Rectangle();

            switch (owner.Navigator.Location)
            {
                case NavigatorLocation.Bottom:
                    pageExtendRect = new Rectangle(0, 0, ClientSize.Width, pageHeight);
                    break;

                case NavigatorLocation.Top:
                    pageExtendRect = new Rectangle(0, NavMinSize.Height - pageHeight, ClientSize.Width, pageHeight);
                    break;
            }

            e.Graphics.FillRectangle(selBrush, pageExtendRect);

            // draw the tab outline
            GraphicsPath path = new GraphicsPath();
            Point drawPoint = new Point(0, pageHeight);

            for (int i = 0; i < visiblePages.Count; i++)
            {
                TabRenderInfo info = tabs[i];
                Rectangle rect = tabs[i].Bounds;
                NotebookPage page = visiblePages[i];
                Font thisFont = this.Font;

                drawPoint = rect.Location;

                switch (owner.Navigator.Location)
                {
                    case NavigatorLocation.Top:
                        drawPoint.Y += tabMargin;
                        break;
                }

                // fill background of the selected page
                if (owner.SelectedPage == page)
                {
                    Rectangle fillRect = new Rectangle();
                    thisFont = mBoldFont;

                    // add the right lines to the path to go around the tab
                    switch (owner.Navigator.Location)
                    {
                        case NavigatorLocation.Top:
                            path.AddLines(new Point[] 
                            {
                                new Point(rect.Left, rect.Bottom),
                                new Point(rect.Left, rect.Top+2),
                                new Point(rect.Left + 2, rect.Top),
                                new Point(rect.Right-2, rect.Top),
                                new Point(rect.Right, rect.Top+2),
                                new Point(rect.Right, rect.Bottom)
                            });

                            fillRect = new Rectangle(
                                rect.Left + 1, rect.Top + 1, rect.Width - 1, rect.Height);

                            break;

                        case NavigatorLocation.Bottom:
                            path.AddLines(new Point[] 
                            {
                                rect.Location,
                                new Point(rect.Left, rect.Bottom-2),
                                new Point(rect.Left + 2, rect.Bottom),
                                new Point(rect.Right-2, rect.Bottom),
                                new Point(rect.Right, rect.Bottom-2),
                                new Point(rect.Right, rect.Top)
                            });

                            fillRect = new Rectangle(
                                rect.Left + 1, rect.Top, rect.Width - 1, rect.Height - 1);

                            break;
                    }

                    e.Graphics.FillRectangle(selBrush, fillRect);

                }
                else
                {
                    switch (owner.Navigator.Location)
                    {
                        case NavigatorLocation.Bottom:
                            path.AddLine(rect.Location, new Point(rect.Right, rect.Top));
                            break;

                        case NavigatorLocation.Top:
                            path.AddLine(new Point(rect.X, rect.Bottom), new Point(rect.Right, rect.Bottom));
                            break;
                    }

                    if (i > 0)
                    {
                        e.Graphics.DrawLine(pathPen, new Point(rect.Left, rect.Top + 4), new Point(rect.Left, rect.Bottom - 4));
                    }           
                }

                drawPoint.X += tabMargin;

                // draw page image
                if (page.Image != null)
                {
                    drawPoint.X += imageMargin;

                    Rectangle imageRect = new Rectangle(drawPoint, info.imageSize);
                    imageRect.Y += (rect.Height - imageRect.Height) / 2;

                    e.Graphics.DrawImage(page.Image, imageRect);

                    drawPoint.X += info.imageSize.Width + imageMargin;
                }

                if (navProperties.AllowClose && info.ShowCloseBox)
                {
                    Image image = Properties.Resources.Close;

                    if (mOverCloseBox == i)
                        image = Properties.Resources.CloseMouseOver;

                    e.Graphics.DrawImage(image, info.CloseBox);
                }

                Rectangle textRect = Rectangle.FromLTRB(
                    drawPoint.X, drawPoint.Y, rect.Right - tabMargin, rect.Bottom - tabMargin - 1);

                TextRenderer.DrawText(e.Graphics, page.Text, thisFont, textRect, ForeColor,
                    TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);


            }

            if (path.PointCount > 0)
            {
                switch (owner.Navigator.Location)
                {
                    case NavigatorLocation.Bottom:
                        path.AddLine(Point.Round(path.GetLastPoint()), new Point(ClientRectangle.Right, drawPoint.Y));
                        break;

                    case NavigatorLocation.Top:
                        path.AddLine(Point.Round(path.GetLastPoint()), new Point(ClientRectangle.Right, TabHeight));
                        break;
                }

                e.Graphics.DrawPath(pathPen, path);
            }

        }

        private void UpdateTabLocations()
        {
            int total = 0;
            Point drawPoint = Point.Empty;

            switch (owner.Navigator.Location)
            {
                case NavigatorLocation.Bottom:
                    drawPoint.Y = pageHeight;
                    break;
            }

            if (tabs == null || tabs.Length != owner.VisiblePageCount)
            {
                if (visiblePages.Count != owner.VisiblePageCount)
                {
                    RefreshAllPages();

                    // return because RefreshAllPages will call this function.
                    return;
                }
                tabs = new TabRenderInfo[owner.VisiblePageCount];

                for (int i = 0; i < tabs.Length; i++)
                    tabs[i] = new TabRenderInfo();
            }

            // calculate the total size used if none of the tabs are compressed.
            for (int i = 0; i < visiblePages.Count; i++)
            {
                TabRenderInfo info = tabs[i];
                Size tabSize, imageSize, textSize;

                TabMetrics(visiblePages[i], out tabSize, out imageSize, out textSize);

                total += tabSize.Width;

                info.Location = drawPoint;
                info.imageSize = imageSize;
                info.textSize = textSize;

                if (navProperties.AllowClose && ParentDesign == false)
                {
                    info.ShowCloseBox = true;
                }
                drawPoint.X += tabSize.Width;
            }

            if (total >= ClientSize.Width)
            {
                int pixelsToReclaim = total - ClientSize.Width + 1;
                int divideCount = tabs.Length;

                if (SelectedIndex >= 0 && divideCount > 1)
                    divideCount -= 1;

                int pixelsReclaimed = 0;

                // remove close boxes first
                for (int i = tabs.Length - 1; i >= 0; i--)
                {
                    if (visiblePages[i] == owner.SelectedPage)
                        continue;

                    if (tabs[i].ShowCloseBox)
                    {
                        tabs[i].ShowCloseBox = false;

                        pixelsReclaimed += Properties.Resources.Close.Width;

                        for (int j = i + 1; j < tabs.Length; j++)
                        {
                            tabs[j].Location.X -= Properties.Resources.Close.Width;
                        }
                    }

                    if (pixelsReclaimed >= pixelsToReclaim)
                        break;
                }

                // shorten text now since removing closeboxes wasn't enough
                if (pixelsReclaimed < pixelsToReclaim)
                {
                    int shorten = (int)Math.Ceiling(pixelsToReclaim / (double)divideCount);

                    for (int i = 0; i < tabs.Length; i++)
                    {
                        if (visiblePages[i] == owner.SelectedPage)
                            continue;


                        tabs[i].textSize.Width -= shorten;

                        for (int j = i + 1; j < tabs.Length; j++)
                        {
                            tabs[j].Location.X -= shorten;
                        }
                    }
                }
            }
        }

        private int TabHeight
        {
            get { return (int)(FontHeight * tabTextHeightRatio) - 1 - pageHeight + tabMargin;}
        }
        private void TabMetrics(NotebookPage notebookPage, out Size tabSize, out Size imageSize, out Size textSize)
        {
            string text = notebookPage.Text;

            if (mBoldFont == null ||
                mBoldFont.FontFamily != Font.FontFamily ||
                mBoldFont.SizeInPoints != Font.SizeInPoints)
            {
                mBoldFont = new Font(Font, FontStyle.Bold);
            }

            Font thisFont = Font;
            if (notebookPage == owner.SelectedPage)
                thisFont = mBoldFont;

            textSize = TextRenderer.MeasureText(text, thisFont);

            textSize.Height = TabHeight;

            Image image = notebookPage.Image;

            // see if we have an image
            if (image != null)
            {
                imageSize = image.Size;

                if (image.Height > textSize.Height)
                {
                    float ratio = textSize.Height / (float)image.Height;

                    imageSize.Width = (int)(image.Width * ratio);
                    imageSize.Height = (int)(image.Height * ratio);
                }
            }
            else
                imageSize = Size.Empty;

            // add close box size
            int closeBox = 0;

            if (navProperties.AllowClose)
            {
                closeBox = imageMargin + Properties.Resources.Close.Width;
            }

            tabSize = new Size(tabMargin + imageSize.Width + imageMargin * 2 + textSize.Width + closeBox + tabMargin,
                               TabHeight);

        }

        class TabRenderInfo
        {
            public Size imageSize;
            public Size textSize;

            public bool ShowCloseBox;
            public Point Location;

            public Rectangle Bounds
            {
                get
                {
                    Rectangle retval = new Rectangle(Location, Size.Empty);

                    retval.Width = tabMargin +
                                   imageMargin + imageSize.Width +
                                   imageMargin + textSize.Width;

                    if (ShowCloseBox)
                        retval.Width += imageMargin + Properties.Resources.Close.Width;

                    retval.Width += tabMargin;

                    retval.Height = Math.Max(imageSize.Height, textSize.Height);
                    
                    return retval;
                }
            }

            private Point CloseBoxLocation
            {
                get
                {
                    Point retval = 
                        new Point(tabMargin + imageMargin + imageSize.Width +
                                   imageMargin + textSize.Width, tabMargin);

                    retval.Y = (Bounds.Height - Properties.Resources.Close.Height) / 2;

                    return retval;
                }
            }
            public Rectangle CloseBox
            {
                get
                {
                    return new Rectangle(
                        new Point(CloseBoxLocation.X + Location.X,
                                  CloseBoxLocation.Y + Location.Y), 
                        Properties.Resources.Close.Size);
                }
            }
        }
    }
}
