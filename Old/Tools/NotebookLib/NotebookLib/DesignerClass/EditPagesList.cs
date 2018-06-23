using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace NotebookLib.DesignerClass
{
    class EditPagesList : DesignerActionList 
    {
        NotebookDesigner designer;

        public EditPagesList(NotebookDesigner designer, IComponent component)
            : base(component)
        {
            this.designer = designer;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection col = new DesignerActionItemCollection();

            col.Add(new DesignerActionMethodItem(this, "AddPage", "Add Page"));

            return col;
        }


        public void AddPage()
        {
            designer.AddPage(null, null);
         
   
        }
    }
}
