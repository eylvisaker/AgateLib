using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Text;

namespace NotebookLib.DesignerClass
{
    class MySplitterDesigner : ControlDesigner 
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return
                    SelectionRules.AllSizeable |
                    SelectionRules.Visible |
                    SelectionRules.Moveable;
            }
        }
        protected override bool GetHitTest(System.Drawing.Point point)
        {
            return true;
        }
    }
}
