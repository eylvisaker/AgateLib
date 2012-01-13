using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib.FlatTabs
{
    class FlatTabProperties : NavigatorProperties 
    {
        public FlatTabProperties(Notebook owner, INavigator nav)
            : base(owner, nav)
        {
            base.Location = NavigatorLocation.Bottom;

            Margin = new Padding(0);
            PageBackColor = SystemColors.ControlLightLight;
        }

        private bool mAllowTabReorder = true;

        [DefaultValue(true)]
        [Description("If true, this allows the user to drag tabs to reorder them.")]
        public bool AllowTabReorder
        {
            get { return mAllowTabReorder; }
            set { mAllowTabReorder = value; }
        }

        public override Padding Margin
        {
            get
            {
                return base.Margin;
            }
            set
            {
                base.Margin = value;
            }
        }
        [DefaultValue(NavigatorLocation.Bottom)]
        public override NavigatorLocation Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                base.Location = value;
            }
        }

        public override bool AllowSplitter
        {
            get
            {
                return false;
            }
        }



    }

}
