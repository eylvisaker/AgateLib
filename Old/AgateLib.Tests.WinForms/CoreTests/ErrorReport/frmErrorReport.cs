using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AgateLib;

namespace AgateLib.Tests.CoreTests.ErrorReport
{
	public partial class frmErrorReportTester : Form
	{
		public frmErrorReportTester()
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
			AgateLib.AgateApp.ErrorReporting.Report(ErrorLevel.Comment, "Test comment about application condition", null);

			try
			{
				System.IO.File.Open("file not there.txt", System.IO.FileMode.Open);
			}
			catch (Exception e)
			{
				AgateLib.AgateApp.ErrorReporting.Report(ErrorLevel.Warning, "File not there.", e);
			}

			try
			{
				System.IO.File.Open("important missing file.dll", System.IO.FileMode.Open);
			}
			catch (Exception e)
			{
				AgateLib.AgateApp.ErrorReporting.Report(ErrorLevel.Fatal, "Missing file", e);
			}

			try
			{
				throw new InvalidOperationException("Something went wrong!");
			}
			catch (Exception e)
			{
				AgateLib.AgateApp.ErrorReporting.Report(ErrorLevel.Bug, "Oops, a bug.", e);
			}
		}

		private void FillTextBox()
		{
			System.IO.TextReader r = new System.IO.StreamReader(AgateLib.AgateApp.ErrorReporting.ErrorFile);

			textBox1.Text = r.ReadToEnd();

			r.Dispose();
		}
	}
}