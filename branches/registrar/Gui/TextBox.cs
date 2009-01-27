using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
    public class TextBox : Widget 
    {
        int mInsertionPoint;

        public int InsertionPoint
        {
            get { return mInsertionPoint; }
            set
            {
                if (value < 0) value = 0;
                if (value > Text.Length) value = Text.Length;

                mInsertionPoint = value;
            }
        }
        protected internal override void SendKeyDown(AgateLib.InputLib.InputEventArgs e)
        {
            ResetIPBlinking();

            switch (e.KeyCode)
            {
                case AgateLib.InputLib.KeyCode.Left:
                    InsertionPoint--;
                    break;

                case AgateLib.InputLib.KeyCode.Right:
                    InsertionPoint++;
                    break;

                case AgateLib.InputLib.KeyCode.Home:
                    InsertionPoint = 0;
                    break;

                case AgateLib.InputLib.KeyCode.End:
                    InsertionPoint = Text.Length;
                    break;

                case AgateLib.InputLib.KeyCode.BackSpace:
                    if (InsertionPoint == 0)
                        break;
                    else if (InsertionPoint == Text.Length)
                        Text = Text.Substring(0, Text.Length - 1);
                    else
                        Text = Text.Substring(0, InsertionPoint - 1) + Text.Substring(InsertionPoint);
                    InsertionPoint--;

                    break;

                case AgateLib.InputLib.KeyCode.Delete:
                    if (InsertionPoint == Text.Length)
                        break;
                    else if (InsertionPoint == 0)
                        Text = Text.Substring(1);
                    else
                        Text = Text.Substring(0, InsertionPoint) + Text.Substring(InsertionPoint + 1);

                    break;

                default:
                    if (InsertionPoint == Text.Length)
                        Text += e.KeyString;
                    else if (InsertionPoint == 0)
                        Text = e.KeyString + Text;
                    else
                        Text = Text.Substring(0, InsertionPoint) + e.KeyString + Text.Substring(InsertionPoint);

                    InsertionPoint++;

                    break;
            }
        }

        protected internal override bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
        {
            switch (keyCode)
            {
                case AgateLib.InputLib.KeyCode.Left:
                case AgateLib.InputLib.KeyCode.Right:
                    return true;
                default:
                    return false;
            }
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                if (InsertionPoint > Text.Length)
                    InsertionPoint = Text.Length;

                ResetIPBlinking();
            }
        }

        private void ResetIPBlinking()
        {
            IPTime = Timing.TotalMilliseconds;
        }
        //public bool MultiLine { get; set; }

        internal double IPTime { get; set; }

    }
}
