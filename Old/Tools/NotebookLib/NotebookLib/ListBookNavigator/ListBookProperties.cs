using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NotebookLib.ListBookNavigator
{
    [Editor(typeof(System.Drawing.Design.UITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    class ListBookProperties : NavigatorProperties 
    {
        public ListBookProperties(Notebook owner, INavigator nav)
            : base(owner, nav)
        {
        }
    }
}
