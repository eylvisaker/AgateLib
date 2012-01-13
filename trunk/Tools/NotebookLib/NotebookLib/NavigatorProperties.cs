using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Editor(typeof(System.Drawing.Design.UITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public abstract class NavigatorProperties
    {
        Notebook owner;
        INavigator navigator;


        public NavigatorProperties(Notebook owner, INavigator navigator)
        {
            this.owner = owner;
            this.navigator = navigator;

            PageBackColor = SystemColors.Control;
        }

        protected Notebook Owner
        {
            get { return owner; }
        }
        protected INavigator Navigator
        {
            get { return navigator; }
        }

        private NavigatorLocation mNavigatorLocation = NavigatorLocation.Left;
        private Padding mNavMargin = new Padding(3);
        private bool mShowNav = true;
        private bool mHideIf;
        private bool mCloseBox = false;

        private Color mPageBackColor;

        [NotifyParentProperty(true)]
        public Color PageBackColor
        {
            get { return mPageBackColor; }
            set { mPageBackColor = value; }
        }
	

        
        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual Padding Margin
        {
            get { return mNavMargin; }
            set
            {
                mNavMargin = value;
                owner.RedoLayout();
            }
        }


        [DefaultValue(false)]
        [Description("Hides the Navigator if the Notebook has one or fewer pages.")]
        public virtual bool HideIfNoChoices
        {
            get { return mHideIf; }
            set
            {
                mHideIf = value;

                owner.RedoLayout();
            }
        }
        [DefaultValue(true)]
        public virtual bool Show
        {
            get
            {
                return mShowNav;
            }
            set
            {
                mShowNav = value;

                owner.RedoLayout();
            }
        }
        [DefaultValue(true)]
        [Browsable(false)]
        public virtual bool IsShowing
        {
            get
            {
                if (mShowNav == false)                    return false;
                else if (HideIfNoChoices && owner.VisiblePageCount < 2) return false;
                else
                    return true;
            }
        }
        [DefaultValue(NavigatorLocation.Left)]
        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual NavigatorLocation Location
        {
            get { return mNavigatorLocation; }
            set
            {
                if (value == mNavigatorLocation)
                    return;

                int oldSplitValue = owner.SplitterLocation;

                switch (mNavigatorLocation)
                {
                    case NavigatorLocation.Right:
                        oldSplitValue = owner.Width - owner.SplitterLocation;
                        break;

                    case NavigatorLocation.Bottom:
                        oldSplitValue = owner.Height - owner.SplitterLocation;
                        break;
                }

                mNavigatorLocation = value;

                owner.SplitterLocation = oldSplitValue;

                switch (mNavigatorLocation)
                {
                    case NavigatorLocation.Right:
                        owner.SplitterLocation = owner.Width - owner.SplitterLocation;
                        break;

                    case NavigatorLocation.Bottom:
                        owner.SplitterLocation = owner.Height - owner.SplitterLocation;
                        break;
                }

                owner.RedoLayout();
            }
        }

        [Browsable(false)]
        public virtual bool AllowSplitter
        {
            get { return true; }
        }

        [DefaultValue(false)]
        [Description("If true, then a close box will be shown on tabs allowing the user to close them.  "
            + "Not all navigators support this property.")]
        public bool AllowClose
        {
            get { return mCloseBox; }
            set { mCloseBox = value; }
        }
	
    }
}
