using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib
{
    [Designer(typeof(DesignerClass.NotebookDesigner))]
    [ToolboxBitmap(typeof(Notebook), "book_reportHS")]
    [DefaultEvent("SelectedPageChanged")]
    public partial class Notebook : UserControl
    {
        NavigatorType mNavType;
        Control mNavigator;
        NavigatorProperties mNavProperties;

        bool mDraggingSplitter;
        int mSplitterDragOffset;

        private int mSplitterWidth = 4;
        private Size mOldSize;

        public Notebook()
        {
            mNotebookPages = new NotebookPageCollection(this);

            System.Reflection.PropertyInfo info = this.GetType().GetProperty("NavigatorType");
            object[] attributes = info.GetCustomAttributes(false);

            foreach (object a in attributes)
            {
                if (a is DefaultValueAttribute)
                {
                    DefaultValueAttribute value = (DefaultValueAttribute)a;

                    CreateNavigator((NavigatorType)value.Value);
                }
            }

            mSplitterLocation = this.Width / 2;
            mOldSize = Size;

            InitializeComponent();
        }

        private void CreateNavigator(NavigatorType type)
        {
            INavigator navigator = NavigatorFactory.CreateNavigator(this, type);
            int currentNavSize = 0;
            int selectedIndex = -1;

            if (navigator is Control == false)
                throw new ArgumentException("Notebook navigators must inherit from Control.");

            if (mNavigator != null)
            {
                selectedIndex = SelectedIndex;

                switch (Navigator.Location)
                {
                    case NavigatorLocation.Left:
                    case NavigatorLocation.Top:
                        currentNavSize = mSplitterLocation;
                        break;

                    case NavigatorLocation.Bottom:
                        currentNavSize = ClientSize.Height - mSplitterLocation;
                        break;

                    case NavigatorLocation.Right:
                        currentNavSize = ClientSize.Width - mSplitterLocation;
                        break;
                }
            }

            mNavType = type;
            mNavProperties = navigator.CreateProperties();
            mNavigator = (Control)navigator;

            // remove any existing navigator before adding the new one
            NoteControls.RemoveNavigator();
            NoteControls.Add(mNavigator);

            NavigatorInterface.RefreshAllPages();
            SelectedIndex = selectedIndex;

            RedoLayout();
            DisplayCorrectPage();
        }
        
        bool doingLayout = false;

        internal void RedoLayout()
        {
            if (doingLayout)
                return;

            try
            {
                doingLayout = true;
                RedoLayoutImpl();
            }
            finally
            {
                doingLayout = false;
            }
        }

        void RedoLayoutImpl()
        {
            Rectangle navRegion;

            if (mNavigator == null)
                return;

            if (SelectedIndex == -1 && NotebookPages.Count > 0)
                SelectedIndex = 0;

            int currentNavSize = 0;

            // make sure the navigator size is within the right range
            switch (Navigator.Location)
            {
                case NavigatorLocation.Left:
                case NavigatorLocation.Right:
                    currentNavSize = mNavigator.Width;

                    if (currentNavSize < NavigatorInterface.NavMinSize.Width)
                        currentNavSize = NavigatorInterface.NavMinSize.Width;

                    if (currentNavSize > NavigatorInterface.NavMaxSize.Width)
                        currentNavSize = NavigatorInterface.NavMaxSize.Width;

                    break;

                case NavigatorLocation.Top:
                case NavigatorLocation.Bottom:
                    currentNavSize = mNavigator.Height;

                    if (currentNavSize < NavigatorInterface.NavMinSize.Height)
                        currentNavSize = NavigatorInterface.NavMinSize.Height;

                    if (currentNavSize > NavigatorInterface.NavMaxSize.Height)
                        currentNavSize = NavigatorInterface.NavMaxSize.Height;

                    break;

            }

            // set the splitter location to size the navigator at the right size
            switch (Navigator.Location)
            {
                case NavigatorLocation.Left:
                    mNavigator.Width = currentNavSize;
                    mSplitterLocation = currentNavSize + Navigator.Margin.Horizontal;
                    break;

                case NavigatorLocation.Right:
                    mNavigator.Width = currentNavSize;
                    mSplitterLocation = ClientSize.Width - currentNavSize - SplitterWidth - Navigator.Margin.Horizontal;
                    break;

                case NavigatorLocation.Top:
                    mNavigator.Height = currentNavSize;
                    mSplitterLocation = currentNavSize + Navigator.Margin.Vertical;
                    break;

                case NavigatorLocation.Bottom:
                    mNavigator.Height = currentNavSize;
                    mSplitterLocation = ClientSize.Height - currentNavSize - SplitterWidth - Navigator.Margin.Vertical;
                    break;

            }

            if (Navigator.IsShowing)
            {
                switch (Navigator.Location)
                {
                    case NavigatorLocation.Left:
                        navRegion = new Rectangle(0, 0,
                                                  mSplitterLocation, ClientSize.Height);

                        mPageRect = new Rectangle(mSplitterLocation + SplitterWidth, 0,
                                                  0, ClientSize.Height);

                        mPageRect.Width = ClientSize.Width - mPageRect.Left;

                        break;

                    case NavigatorLocation.Right:
                        mPageRect = new Rectangle(0, 0,
                                                  mSplitterLocation, ClientSize.Height);

                        navRegion = new Rectangle(mSplitterLocation + SplitterWidth, 0,
                                                  0, ClientSize.Height);

                        navRegion.Width = ClientSize.Width - navRegion.Left;

                        break;

                    case NavigatorLocation.Top:
                        navRegion = new Rectangle(0, 0,
                                                  ClientSize.Width, mSplitterLocation);

                        mPageRect = new Rectangle(0, mSplitterLocation + SplitterWidth,
                                                  ClientSize.Width, 0);

                        mPageRect.Height = ClientSize.Height - mPageRect.Top;

                        break;

                    case NavigatorLocation.Bottom:
                        mPageRect = new Rectangle(0, 0,
                                                  ClientSize.Width, mSplitterLocation);

                        navRegion = new Rectangle(0, mSplitterLocation + SplitterWidth,
                                                  ClientSize.Width, 0);

                        navRegion.Height = ClientSize.Height - navRegion.Top;

                        break;

                    default:
                        throw new Exception("Navigator.Location is not set to a valid value.");
                }

            }
            else
            {
                navRegion = new Rectangle(-1000, -1000, 50, 50);
                mPageRect = this.ClientRectangle;
            }

            mNavigator.Location = GetLocationWithMargin(navRegion, Navigator.Margin);
            mNavigator.Size = GetSizeWithMargin(navRegion, Navigator.Margin);

            NavigatorInterface.OnUpdateLayout();

            foreach (NotebookPage p in NotebookPages)
            {
                p.Location = mPageRect.Location;
                p.Size = mPageRect.Size;
            }

            DisplayCorrectPage();
        }

        #region --- Events ---


        public event EventHandler SelectedPageChanged;

        public event ClosePageQueryHandler ClosePageQuery;
        public event ClosePageHandler ClosePage;

        internal void OnCloseTab(NotebookPage page)
        {
            ClosePageQueryEventArgs args = new ClosePageQueryEventArgs(page);

            if (ClosePageQuery != null)
                ClosePageQuery(this, args);

            if (args.Cancel == false)
            {
                RemovePageInternal(page);


                if (ClosePage != null)
                    ClosePage(this, new ClosePageEventArgs(page));
            }


        }
        internal void OnSelectedPageChanged()
        {
            if (NoteControls.Count == 1)
                return;

            DisplayCorrectPage();

            if (SelectedPageChanged != null)
                SelectedPageChanged(this, EventArgs.Empty);
        }

        #endregion
        #region --- NotebookPage Management ---

        /// <summary>
        /// Replace the Controls collection with my own inherited version of it.
        /// </summary>
        private NoteControlCollection NoteControls
        {
            get { return (NoteControlCollection)base.Controls; }
        }

        /// <summary>
        /// Creates a NoteControlCollection instance.
        /// </summary>
        /// <returns></returns>
        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new NoteControlCollection(this);
        }

        private NotebookPageCollection mNotebookPages;

        [Browsable(false)]
        public NotebookPageCollection NotebookPages
        {
            get { return mNotebookPages; }
        }

        private void AddPageInternal(NotebookPage page)
        {
            if (page == null)
                throw new ArgumentException("Page cannot be null.");
            if (NotebookPages.Contains(page))
                throw new ArgumentException("Page is already in the notebook.");

            NotebookPages.AddInternal(page);
            NoteControls.AddInternal(page);

            NavigatorInterface.RefreshAllPages();

        }
        private void RemovePageInternal(NotebookPage page)
        {
            if (page == null)
                throw new NullReferenceException();

            int newIndex = SelectedIndex;

            if (newIndex >= NotebookPages.IndexOf(page) && newIndex > -1)
            {
                newIndex--;

                if (newIndex < 0 && NotebookPages.Count > 1)
                    newIndex = 0;
            }
            if (newIndex >= NotebookPages.Count)
                newIndex = NotebookPages.Count - 1;

            SelectedIndex = newIndex;

            NotebookPages.RemoveInternal(page);
            NoteControls.RemoveInternal(page);

            if (NotebookPages.Count > 0 && SelectedIndex == -1)
                SelectedIndex = 0;

            NavigatorInterface.RefreshAllPages();

            DisplayCorrectPage();
        }
        private void InsertPageInternal(int index, NotebookPage page)
        {
            if (page == null)
                throw new NullReferenceException();
            if (NotebookPages.Contains(page))
                throw new ArgumentException();

            NotebookPages.InsertInternal(index, page);
            NoteControls.AddInternal(page);

            NavigatorInterface.RefreshAllPages();
        }

        [Browsable(false)]
        public int PageCount
        {
            get { return mNotebookPages.Count; }
        }


        #endregion
        #region --- Navigator Management ---

        [DefaultValue(NavigatorType.FlatTabs)]
        [NotifyParentProperty(true)]
        public NavigatorType NavigatorType
        {
            get { return mNavType; }
            set
            {
                if (value != mNavType)
                {
                    CreateNavigator(value);
                }
            }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public NavigatorProperties Navigator
        {
            get { return mNavProperties; }
        }

        internal Control NavigatorControl
        {
            get { return mNavigator; }
        }
        [Browsable(false)]
        internal INavigator NavigatorInterface
        {
            get
            {
                return (INavigator)mNavigator;
            }
        }

        #endregion
        #region --- Custom Painting ---

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (NotebookPages.Count == 0 || SelectedIndex == -1)
            {
                e.Graphics.FillRectangle(SystemBrushes.AppWorkspace,
                    mPageRect);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

        }

        #endregion

        #region --- General Properties ---

        private int mSplitterLocation;


        [DefaultValue(4)]
        public int SplitterWidth
        {
            get
            {
                if (Navigator.AllowSplitter == false)
                    return 0;

                return mSplitterWidth;

            }
            set
            {
                if (value < 0 || value > 10)
                    throw new ArgumentException();

                mSplitterWidth = value;
            }
        }

        public int SplitterLocation
        {
            get { return mSplitterLocation; }
            set
            {
                mSplitterLocation = value;
                SizeNavFromSplitter();
            }
        }

        private void SizeNavFromSplitter()
        {
            if (Navigator == null)
                return;

            switch (Navigator.Location)
            {
                case NavigatorLocation.Left:
                    mNavigator.Width = SplitterLocation;
                    break;

                case NavigatorLocation.Right:
                    mNavigator.Width = Width - SplitterLocation - SplitterWidth;
                    break;

                case NavigatorLocation.Top:
                    mNavigator.Height = SplitterLocation;
                    break;

                case NavigatorLocation.Bottom:
                    mNavigator.Height = Height - SplitterLocation - SplitterWidth;
                    break;
            }

            RedoLayout();
        }

        #endregion

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            RedoLayout();
        }



        private Rectangle mPageRect;
        internal Rectangle PageRect
        {
            get { return mPageRect; }
        }

        private Size GetSizeWithMargin(Rectangle rect, Padding margin)
        {
            Size loc = new Size(
                rect.Width - margin.Horizontal,
                rect.Height - margin.Vertical);

            return loc;
        }
        private Point GetLocationWithMargin(Rectangle rect, Padding margin)
        {
            Point loc = new Point(
                rect.Left + margin.Left,
                rect.Top + margin.Top);
            return loc;
        }

        private void DisplayCorrectPage()
        {
            foreach (NotebookPage p in NotebookPages)
            {
                p.Visible = false;
            }

            if (NotebookPages.Count == 0)
            {
                SelectedIndex = -1;
                return;
            }

            if (NavigatorInterface.SelectedIndex == -1)
                return;

            SelectedPage.BringToFront();
            SelectedPage.Visible = true;

            SelectedPage.BackColor = Navigator.PageBackColor;

            Invalidate();
        }

        [RefreshProperties( RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public NotebookPage SelectedPage
        {
            get
            {
                if (NotebookPages.Count == 0)
                    return null;
                if (NavigatorInterface.SelectedIndex == -1)
                    return null;
                
                return NotebookPages[NavigatorInterface.SelectedIndex];
            }
            set
            {
                if (NotebookPages.Contains(value) == false)
                {
                    throw new InvalidOperationException(
                        "Cannot set SelectedPage to a NotebookPage which is " +
                        "not in this control.");
                }

                NavigatorInterface.SelectedIndex = NotebookPages.IndexOf(value);
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        public int SelectedIndex
        {
            get { return NavigatorInterface.SelectedIndex; }
            set
            {
                NavigatorInterface.SelectedIndex = value;

                RedoLayout();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int oldLoc;

            switch (Navigator.Location)
            {
                case NavigatorLocation.Right:
                    oldLoc = mOldSize.Width - mSplitterLocation;
                    mSplitterLocation = Size.Width - oldLoc;
                    break;

                case NavigatorLocation.Bottom:
                    oldLoc = mOldSize.Height - mSplitterLocation;
                    mSplitterLocation = Size.Height - oldLoc;
                    break;
            }

            mOldSize = Size;

            RedoLayout();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mDraggingSplitter)
            {
                int split = SplitterLocation;

                switch (Navigator.Location)
                {
                    case NavigatorLocation.Left:
                        split = e.X - mSplitterDragOffset;

                        if (split < NavigatorInterface.NavMinSize.Width)
                            split = NavigatorInterface.NavMinSize.Width;
                        if (split > NavigatorInterface.NavMaxSize.Width)
                            split = NavigatorInterface.NavMaxSize.Width;

                        Cursor.Current = Cursors.VSplit;
                        
                        break;
                    
                    case NavigatorLocation.Right:
                        split = e.X - mSplitterDragOffset;

                        if (split < ClientSize.Width - SplitterWidth - NavigatorInterface.NavMaxSize.Width)
                            split = ClientSize.Width - SplitterWidth - NavigatorInterface.NavMaxSize.Width;
                        if (split > ClientSize.Width - SplitterWidth - NavigatorInterface.NavMinSize.Width)
                            split = ClientSize.Width - SplitterWidth - NavigatorInterface.NavMinSize.Width;

                        Cursor.Current = Cursors.VSplit;

                        break;

                    case NavigatorLocation.Top:
                        split = e.Y - mSplitterDragOffset;

                        if (split < NavigatorInterface.NavMinSize.Height)
                            split = NavigatorInterface.NavMinSize.Height;
                        if (split > NavigatorInterface.NavMaxSize.Height)
                            split = NavigatorInterface.NavMaxSize.Height;

                        Cursor.Current = Cursors.HSplit;

                        break;

                    case NavigatorLocation.Bottom:
                        split = e.Y - mSplitterDragOffset;

                        if (split < ClientSize.Height - SplitterWidth - NavigatorInterface.NavMaxSize.Height)
                            split = ClientSize.Height - SplitterWidth - NavigatorInterface.NavMaxSize.Height;
                        if (split > ClientSize.Height - SplitterWidth - NavigatorInterface.NavMinSize.Height)
                            split = ClientSize.Height - SplitterWidth - NavigatorInterface.NavMinSize.Height;

                        Cursor.Current = Cursors.HSplit;

                        break;
                }

                SplitterLocation = split;
            }
            else
            {
                if (MouseInSplitter(e.Location))
                {
                    switch (Navigator.Location)
                    {
                        case NavigatorLocation.Left:
                        case NavigatorLocation.Right:
                            Cursor.Current = Cursors.VSplit;
                            break;

                        case NavigatorLocation.Top:
                        case NavigatorLocation.Bottom:
                            Cursor.Current = Cursors.HSplit;
                            break;

                        default:
                            Cursor.Current = Cursors.No;
                            break;
                    }
                }
                else
                    Cursor.Current = Cursors.Default;
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (MouseInSplitter(e.Location))
            {
                mDraggingSplitter = true;
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            mDraggingSplitter = false;
            base.OnMouseUp(e);
        }

        private bool MouseInSplitter(Point location)
        {
            if (DesignMode == false && Navigator.AllowSplitter == false)
                return false;

            // splitter doesn't exist if navigator is not shown
            if (Navigator.IsShowing == false)
                return false;

            switch (Navigator.Location)
            {
                case NavigatorLocation.Left:
                case NavigatorLocation.Right:
                    if (location.X >= mSplitterLocation && location.X < mSplitterLocation + SplitterWidth)
                    {
                        mSplitterDragOffset = location.X - mSplitterLocation;
                        return true;
                    }
                    else
                        return false;

                case NavigatorLocation.Top:
                case NavigatorLocation.Bottom:
                    if (location.Y >= mSplitterLocation && location.Y < mSplitterLocation + SplitterWidth)
                    {
                        mSplitterDragOffset = location.Y - mSplitterLocation;
                        return true;
                    }
                    else
                        return false;
            }

            throw new Exception("NavigatorLocation not set to a valid value.");
        }

        internal new bool DesignMode { get { return base.DesignMode; } }

        public NotebookPage[] VisiblePages()
        {
            List<NotebookPage> retval = new List<NotebookPage>();

            if (DesignMode)
            {
                retval.AddRange(NotebookPages);
            }
            else
            {
                for (int i = 0; i < NotebookPages.Count; i++)
                {
                    if (NotebookPages[i].ShowPage)
                        retval.Add(NotebookPages[i]);
                }
            }

            return retval.ToArray();
        }
        public int VisiblePageCount
        {
            get
            {
                if (DesignMode)
                    return NotebookPages.Count;

                int count = 0;

                foreach (NotebookPage p in NotebookPages)
                {
                    if (p.ShowPage)
                        count++;
                }

                return count;
            }
        }

        [Bindable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        internal void RefreshChildLists()
        {
            if (NotebookPages.IsSorted == false)
                NotebookPages.Sort();

            NavigatorInterface.RefreshAllPages();
        }

        public class NotebookPageCollection : IList<NotebookPage>, System.Collections.ICollection
        {
            Notebook owner;
            List<NotebookPage> pages = new List<NotebookPage>();

            internal void Sort()
            {
                pages.Sort();
            }

            internal NotebookPageCollection(Notebook owner)
            {
                this.owner = owner;
            }

            internal bool IsSorted
            {
                get
                {
                    int lastOrder = pages[0].Order;

                    for (int i = 1; i < pages.Count; i++)
                    {
                        if (pages[i].Order >= lastOrder)
                        {
                            lastOrder = pages[i].Order;
                        }
                        else
                            return false;
                    }

                    return true;
                }
            }
            #region --- Internal Add, Insert and Remove actually carry out the action ---

            internal void AddInternal(NotebookPage item)
            {
                if (pages.Count > 0)
                {
                    if (item.OrderValueUserSet == false)
                    {
                        item.Order = pages[pages.Count - 1].Order + 10;
                    }
                }

                pages.Add(item);
                pages.Sort();
            }
            internal void RemoveInternal(NotebookPage item)
            {
                pages.Remove(item);
            }
            internal void InsertInternal(int index, NotebookPage item)
            {
                pages.Insert(index, item);
                pages.Sort();
            }

            #endregion

            #region IList<NotebookPage> Members

            public int IndexOf(NotebookPage item)
            {
                return pages.IndexOf(item);
            }
            public void Insert(int index, NotebookPage item)
            {
                owner.InsertPageInternal(index, item);
            }
            public void RemoveAt(int index)
            {
                Remove(pages[index]);
            }

            public NotebookPage this[int index]
            {
                get
                {
                    return pages[index];
                }
                set
                {
                    pages[index] = value;
                }
            }

            #endregion
            #region ICollection<NotebookPage> Members

            public void Add(NotebookPage item)
            {
                // delegate the Add operation to the Notebook so that
                // the page gets added to the Controls collection as well.
                owner.AddPageInternal(item);
            }

            public void Clear()
            {
                while (pages.Count > 0)
                    RemoveAt(0);
            }
            public bool Contains(NotebookPage item)
            {
                return pages.Contains(item);
            }
            public void CopyTo(NotebookPage[] array, int arrayIndex)
            {
                pages.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return pages.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(NotebookPage item)
            {
                if (this.Contains(item) == false)
                    return false;

                owner.RemovePageInternal(item);

                return true;
            }

            #endregion
            #region IEnumerable<NotebookPage> Members

            public IEnumerator<NotebookPage> GetEnumerator()
            {
                return pages.GetEnumerator();
            }

            #endregion
            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            #region ICollection Members

            void System.Collections.ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            int System.Collections.ICollection.Count
            {
                get { return Count; }
            }

            bool System.Collections.ICollection.IsSynchronized
            {
                get { return false; }
            }

            object System.Collections.ICollection.SyncRoot
            {
                get { return this; }
            }

            #endregion

        }
        public class NoteControlCollection : Control.ControlCollection
        {
            Notebook owner;


            internal void AddInternal(NotebookPage page)
            {
                base.Add(page);
            }
            internal void RemoveInternal(NotebookPage page)
            {
                base.Remove(page);
            }

            internal NoteControlCollection(Notebook owner)
                : base(owner)
            {
                this.owner = owner;
            }
            public override void Add(Control value)
            {
                if (value is NotebookPage)
                {
                    // delegate the addition to the owner, which will call AddInternal.
                    owner.AddPageInternal(value as NotebookPage);
                }
                else
                {
                    if (value is INavigator && HasNavigator == false)
                    {
                        base.Add(value);
                    }
                    else
                        throw new Exception(string.Format(
                            "Cannot add control '{0}' to Notebook.  Only NotebookPages can be added to Notebooks.",
                            value.GetType().Name));
                }
            }
            public override void Remove(Control value)
            {
                if (value is NotebookPage)
                {
                    // delegate the removal to the owner, which will call RemoveInternal
                    owner.RemovePageInternal(value as NotebookPage);
                }
                else
                    base.Remove(value);
            }
            internal bool HasNavigator
            {
                get
                {
                    foreach (Control c in this)
                    {
                        if (c is INavigator)
                            return true;
                    }

                    return false;
                }
            }

            internal void RemoveNavigator()
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i] is INavigator)
                    {
                        RemoveAt(i);
                        i--;
                    }
                }
            }
        }


        /// <summary>
        /// Moves to the next visible page.  If the notebook is on the last visible page,
        /// no changes are made.
        /// Returns true if there was a change to SelectedIndex.
        /// </summary>
        public bool MoveNextPage()
        {
            for (int i = SelectedIndex + 1; i < NotebookPages.Count; i++)
            {
                if (NotebookPages[i].ShowPage)
                {
                    SelectedIndex = i;
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Moves to the previous visible page.  If the notebook is on the first visible page,
        /// then no changes are made.
        /// Returns true if there was a change to SelectedIndex.
        /// </summary>
        public bool MovePrevPage()
        {
            for (int i = SelectedIndex - 1; i >= 0; i--)
            {
                if (NotebookPages[i].ShowPage)
                {
                    SelectedIndex = i;
                    return true;
                }
            }

            return false;
        }
    }
}