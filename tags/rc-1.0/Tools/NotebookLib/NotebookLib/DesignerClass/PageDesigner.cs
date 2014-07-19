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
    class PageDesigner : ParentControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return
                 SelectionRules.Visible |
                 SelectionRules.Moveable;
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                return new DesignerActionListCollection();
            }
        }
        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            return false;
           
        }
        internal void OnDragDropInternal(DragEventArgs de)
        {
            this.OnDragDrop(de);
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                return new DesignerVerbCollection();
            }
        }
    }
}
