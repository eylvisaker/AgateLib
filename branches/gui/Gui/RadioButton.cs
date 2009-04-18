using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
    public class RadioButton : Widget 
    {
        public RadioButton()
        {
            Name = "RadioButton";
        }
        public RadioButton(string text)
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
                if (value == mChecked)
                    return;

                mChecked = value;

                if (value == true && Parent != null)
                {
                    foreach (Widget w in Parent.Children)
                    {
                        RadioButton rad = w as RadioButton;

                        if (rad == null || rad == this)
                            continue;

                        rad.Checked = false;
                    }
                }

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
