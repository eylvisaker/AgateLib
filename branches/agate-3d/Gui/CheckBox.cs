using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
    public class CheckBox : Widget 
    {
        public CheckBox()
        {
            Name = "Checkbox";
        }
        public CheckBox(string text)
        {
            Name = text;
            Text = text;
        }
        private bool mChecked;

        public bool Checked
        {
            get { return mChecked; }
            set
            {
                mChecked = value;

                OnCheckChanged();
            }
        }


        bool mouseDownIn;
        protected internal override void OnMouseDown(AgateLib.InputLib.InputEventArgs e)
        {
            if (Enabled == false)
                return;

            mouseDownIn = true;
        }
        protected internal override void OnMouseUp(AgateLib.InputLib.InputEventArgs e)
        {
            if (MouseIn && mouseDownIn)
                Checked = !Checked;

            mouseDownIn = false;

        }


        private void OnCheckChanged()
        {
            if (CheckChanged != null)
                CheckChanged(this, EventArgs.Empty);
        }

        public event EventHandler CheckChanged;
    }
}
