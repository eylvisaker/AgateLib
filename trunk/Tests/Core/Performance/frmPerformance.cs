// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tests.PerformanceTester
{
    public partial class frmPerformanceTester : Form
    {
        Listener listen;

        public class Listener : TraceListener
        {
            frmPerformanceTester frm;

            public Listener(frmPerformanceTester frm)
            {
                this.frm = frm;
            }

            public override void Write(string message)
            {
                if (frm.IsDisposed == false)
                {
                    if (NeedIndent)
                    {
                        for (int i = 0; i < IndentLevel * IndentSize; i++)
                            message = " " + message;
                    }

                    try
                    {
                        frm.textBox1.AppendText(message);
                    }
                    catch
                    {

                    }

                }
            }

            public override void WriteLine(string message)
            {
                Write(message + "\r\n");

            }
        }
        public frmPerformanceTester()
        {
            InitializeComponent();

            listen = new Listener(this);
            Trace.Listeners.Add(listen);

            this.Location = new Point(Screen.GetBounds(this).Width - this.Width - 10, 10);
        }
    }
}