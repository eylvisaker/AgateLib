using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib.NoNavigator
{
    class NoNavigator : Control, INavigator
    {
        Notebook owner;
        int mSelectedIndex;

        public NoNavigator(Notebook owner)
        {
            this.owner = owner;
        }

        public int SelectedIndex
        {
            get
            {
                return mSelectedIndex;
            }
            set
            {
                mSelectedIndex = value;

                owner.RedoLayout();
                owner.OnSelectedPageChanged();
            }
        }
        public void RefreshAllPages()
        {
            
        }

        public NavigatorProperties CreateProperties()
        {
            return new NoNavigatorProperties(owner, this);
        }

        public System.Drawing.Size NavMinSize
        {
            get { return new System.Drawing.Size(0, 0); }
        }
        public System.Drawing.Size NavMaxSize
        {
            get { return new System.Drawing.Size(0, 0); }
        }

        public void OnUpdateLayout()
        {
            
        }
    }
}
