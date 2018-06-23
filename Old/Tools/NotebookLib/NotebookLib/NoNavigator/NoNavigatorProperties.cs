using System;
using System.Collections.Generic;
using System.Text;

namespace NotebookLib.NoNavigator
{
    class NoNavigatorProperties : NavigatorProperties 
    {
        public NoNavigatorProperties(Notebook owner, INavigator nav)
            : base(owner, nav)
        {
        }

        public override System.Windows.Forms.Padding Margin
        {
            get
            {
                return new System.Windows.Forms.Padding(0, 0, 0, 0);
            }
            set
            {
                base.Margin = value;
            }
        }

        public override bool AllowSplitter
        {
            get
            {
                return false;
            }
        }
        public override bool IsShowing
        {
            get
            {
                return false;
            }
        }
    }
}
