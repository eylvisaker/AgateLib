using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.GuiBase;

namespace ERY.AgateLib.Gui
{
    public class Label : Component
    {
        private string mText;
        private bool mMouseIn;

        public Label()
        {
            BackColor = Color.FromArgb(0, 0, 0, 0);
            AttachEvents();
        }
        public Label(string text)
        {
            BackColor = Color.FromArgb(0, 0, 0, 0);
            Text = text;

            AttachEvents();
        }
        public Label(Container parent, Point location, string text)
            : base(parent, true)
        {
            BackColor = Color.FromArgb(0, 0, 0, 0);
            Text = text;
            Location = location;

            AttachEvents();
        }

        public string Text
        {
            get { return mText; }
            set
            {
                bool evt = !string.Equals(mText, value, StringComparison.Ordinal);
                
                mText = value;

                if (evt)
                    OnTextChanged();

                CheckAutoSize();
            }
        }


        private void AttachEvents()
        {
            this.MouseDown += new GuiInputEventHandler(Label_MouseDown);
            this.MouseUp += new GuiInputEventHandler(Label_MouseUp);
        }

        void Label_MouseUp(object sender, InputEventArgs e)
        {
            if (mMouseIn && this.Bounds.Contains(e.MousePosition))
                OnClick();

            mMouseIn = false;
        }
        void Label_MouseDown(object sender, InputEventArgs e)
        {
            mMouseIn = true;
        }


        private void OnTextChanged()
        {
            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
        }
        private void OnClick()
        {
            if (Click != null)
                Click(this, EventArgs.Empty);
        }

        public event EventHandler Click;
        public event EventHandler TextChanged;
    }
}
