using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERY.AgateLib;

namespace ErrorReportTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateErrors();

            FillTextBox();
        }

        private static void GenerateErrors()
        {
            Core.ReportError(new Exception("Test comment about application condition"), ErrorLevel.Comment);

            try
            {
                System.IO.File.Open("file not there.txt", System.IO.FileMode.Open);
            }
            catch (Exception e)
            {
                Core.ReportError(e, ErrorLevel.Warning);
            }

            try
            {
                System.IO.File.Open("important missing file.dll", System.IO.FileMode.Open);
            }
            catch (Exception e)
            {
                Core.ReportError(e, ErrorLevel.Fatal );
            }

            try
            {
                throw new InvalidOperationException("Something went wrong!");
            }
            catch (Exception e)
            {
                Core.ReportError(e, ErrorLevel.Bug);
            }

        }

        private void FillTextBox()
        {
            System.IO.TextReader r = new System.IO.StreamReader(Core.ErrorFile);

            textBox1.Text = r.ReadToEnd();

            r.Dispose();
        }
    }
}