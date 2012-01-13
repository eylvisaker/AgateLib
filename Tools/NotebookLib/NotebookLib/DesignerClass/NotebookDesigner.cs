using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace NotebookLib.DesignerClass
{
    class NotebookDesigner : ParentControlDesigner
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

        protected override bool GetHitTest(Point point)
        {
            Control c = MyNotebook.GetChildAtPoint(MyNotebook.PointToClient(point));

            if (c is NotebookPage)
                return true;

            // select the notebook before we forward mouse events to the notebook and/or navigator.
            ISelectionService select = (ISelectionService)GetService(typeof(ISelectionService));

            if (select.GetComponentSelected(MyNotebook))
            {
                return true;
            }
            else
                return false;

        }
        protected override bool AllowControlLasso
        {
            get
            {
                return false;
            }
        }

        public override bool CanParent(ControlDesigner controlDesigner)
        {
            if (controlDesigner is PageDesigner)
                return true;
            else
                return false;
        }
        
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
        }
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            AddPage(null, null);
            AddPage(null, null);
        }
 
        public Notebook MyNotebook
        {
            get { return (Notebook)base.Control; }
        }

        DesignerVerbCollection myVerbs;
        DesignerVerb removeVerb;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (myVerbs == null)
                {
                    myVerbs = new DesignerVerbCollection();

                    removeVerb = new DesignerVerb("Remove Page", new EventHandler(RemovePage));

                    myVerbs.Add(new DesignerVerb("Add Page", new EventHandler(AddPage)));
                    myVerbs.Add(removeVerb);
                }
                 
                EnableRemove();

                return myVerbs;
            }
        }
       
        internal void AddPage(object sender, EventArgs e)
        {
            IComponentChangeService iccs = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            IDesignerHost designHost = (IDesignerHost)GetService(typeof(IDesignerHost));

            DesignerTransaction designerTransaction = null;

            // calculate an order value for this tab to keep them in order.
            int order = 0;

            if (MyNotebook.NotebookPages.Count > 0)
            {
                order = MyNotebook.NotebookPages[MyNotebook.NotebookPages.Count - 1].Order + 10;
            }

            try
            {
                designerTransaction = designHost.CreateTransaction("Add Notebook Page");

                NotebookPage tabPage = (NotebookPage)designHost.CreateComponent(typeof(NotebookPage));

                tabPage.Text = tabPage.Name;

                
                tabPage.Order = order;

                MyNotebook.Controls.Add(tabPage);
                MyNotebook.SelectedIndex = MyNotebook.PageCount - 1;

                iccs.OnComponentChanged(Component, null, null, null);

                designerTransaction.Commit();
            }
            finally
            {
                (designerTransaction as IDisposable).Dispose();
            }
        }
        internal void RemovePage(object sender, EventArgs e)
        {
            IComponentChangeService iccs = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            IDesignerHost designHost = (IDesignerHost)GetService(typeof(IDesignerHost));

            DesignerTransaction designerTransaction = null;

            try
            {
                designerTransaction = designHost.CreateTransaction("Remove Notebook Page");

                NotebookPage tabPage = MyNotebook.SelectedPage;

                if (tabPage == null)
                    return;

                MyNotebook.Controls.Remove(tabPage);

                iccs.OnComponentChanged(Component, null, null, null);

                designerTransaction.Commit();

                EnableRemove();
            }
            finally
            {
                (designerTransaction as IDisposable).Dispose();
            }

            
        }

        private void EnableRemove()
        {
            if (MyNotebook.NotebookPages.Count == 0)
                removeVerb.Enabled = false;
            else
                removeVerb.Enabled = true;
        }
    }
}
