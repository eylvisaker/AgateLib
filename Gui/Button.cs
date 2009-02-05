using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.InputLib;

namespace AgateLib.Gui
{
    public class Button : Widget 
    {
        public Button() { Name = "Button"; }
        public Button(string text) { Name = text; Text = text; }

        bool mouseDownIn = false;

        internal bool DrawActivated
        {
            get { return mouseDownIn && MouseIn; }
        }
        internal bool IsDefaultButton
        {
            get
            {
                Container p = this.Parent;

                while (p is Window == false)
                    p = p.Parent;

                return ((Window)p).AcceptButton == this;
            }
        }

        protected internal override void OnMouseDown(InputEventArgs e)
        {
            if (Enabled == false)
                return;

            mouseDownIn = true;
        }
        protected internal override void OnMouseUp(InputEventArgs e)
        {
            mouseDownIn = false;

            if (MouseIn)
                OnClick();
        }

        private void OnClick()
        {
            if (Click != null)
                Click(this, EventArgs.Empty);
        }
        public event EventHandler Click;
    }
}
