using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib
{
    [ToolboxItem(false)]
    [Designer(typeof(DesignerClass.PageDesigner))]
    [Docking(DockingBehavior.Never)]
    public sealed partial class NotebookPage : Panel, IComparable<NotebookPage>
    {
        bool mOrderValueUserSet = false;
        internal bool OrderValueUserSet { get { return mOrderValueUserSet; } }

        public NotebookPage()
        {
            InitializeComponent();

            Text = "Untitled Page";
        }
        public NotebookPage(string text)
        {
            InitializeComponent();

            Text = text;
        }

        public override string ToString()
        {
            return "NotebookPage :  Text: \"" + Text + "\"" + " Order: " + Order.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Bindable(false)]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = DockStyle.None;
            }
        }

        bool fixingLocation;

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);

            if (fixingLocation)
                return;

            try
            {
                fixingLocation = true;

                Notebook n = (Notebook)Parent;

                if (n != null)
                    this.Location = n.PageRect.Location;
            }
            finally
            {
                fixingLocation = false;
            }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (DesignMode)
            {
                Pen p = new Pen(Color.Black);
                p.DashPattern = new float[] { 2, 2 };

                Rectangle rect = ClientRectangle;
                rect.Width--;
                rect.Height--;

                pe.Graphics.DrawRectangle(p, rect);
            }

            base.OnPaint(pe);
        }

        #region --- Hidden properties to reveal ---

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                RefreshParent();
            }
        }
        

        #endregion
        #region --- Properties to hide---

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Bindable(false)]
        public new bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        #endregion

        #region --- NotebookPage Properties ---

        private Image mImage;
        private int mOrder;
        private bool mShowPage = true;

        public Image Image
        {
            get { return mImage; }
            set { mImage = value; }
        }

        /// <summary>
        /// Gets or sets a value which determines how pages should be ordered in 
        /// the Notebook.  Lower values will rise to the top.  Pages with the same Order
        /// value may be reordered by the designer.
        /// </summary>
        [Description("Notebook pages are ordered by the Order value, lowest values first.")]
        public int Order
        {
            get { return mOrder; }
            set
            {
                mOrder = value;

                mOrderValueUserSet = true;

                RefreshParent();
            }
        }

        [DefaultValue(true)]
        public bool ShowPage
        {
            get { return mShowPage; }
            set
            {
                mShowPage = value;

                RefreshParent();
            }
        }

        #endregion

        private void RefreshParent()
        {
            if (Parent == null)
                return;

            (Parent as Notebook).RefreshChildLists();
        }


        #region IComparable<NotebookPage> Members

        int IComparable<NotebookPage>.CompareTo(NotebookPage other)
        {
            return Order.CompareTo(other.Order);
        }

        #endregion
    }
}