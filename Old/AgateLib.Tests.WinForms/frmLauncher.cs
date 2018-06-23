using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AgateLib.Tests
{
	public partial class frmLauncher : Form
	{
		readonly Type[] interfaces = new Type[] { typeof(IAgateTest), };

		Font bold;

		public frmLauncher()
		{
			InitializeComponent();

			Icon = AgateLib.Platform.WinForms.Controls.FormUtil.AgateLibIcon;

			bold = new Font(lstTests.Font, FontStyle.Bold | FontStyle.Italic);
		}

		public event EventHandler<TestEventArgs> LaunchTest;

		public IList<TestInfo> Tests { get; set; }

		public string CommandLine
		{
			get { return txtCommandLine.Text; }
			set { txtCommandLine.Text = value; }
		}

		public void HideBeforeTest()
		{
			Hide();
		}

		public void ShowAfterTest()
		{
			this.Show();
			Cursor.Show();
			this.TopMost = true;
			this.TopMost = false;
			this.Activate();
		}

		public void TestExceptionMessage(string message, string caption)
		{
			MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		public void TestCantRun(string message, string caption)
		{
			MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		private void FillList()
		{
			string lastCategory = null;

			foreach (var test in Tests)
			{
				if (test.Category != lastCategory)
				{
					lstTests.Items.Add(test.Category);
					lastCategory = test.Category;
				}

				this.lstTests.Items.Add(test);
			}
		}

		private void AddTest(Type t)
		{
		}

		private void lstTests_DoubleClick(object sender, EventArgs e)
		{
			TestInfo t = lstTests.SelectedItem as TestInfo;
			if (t == null)
				return;

			OnLaunchTest(t);
		}

		private void lstTests_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				TestInfo t = lstTests.SelectedItem as TestInfo;
				if (t == null)
					return;

				OnLaunchTest(t);
			}
		}

		private void OnLaunchTest(TestInfo t)
		{
			LaunchTest?.Invoke(this, new Tests.TestEventArgs { Info = t });
		}

		private void lstTests_DrawItem(object sender, DrawItemEventArgs e)
		{
			object item = e.Index >= 0 ? lstTests.Items[e.Index] : null;
			const int indent = 15;

			e.DrawBackground();

			PointF loc = e.Bounds.Location;
			Brush b = new SolidBrush(e.ForeColor);

			if (item is string)
			{
				e.Graphics.DrawString((string)item, bold, b, loc);
			}
			else if (item is TestInfo)
			{
				TestInfo t = item as TestInfo;
				loc.X += indent;
				string text = t.Name;

				e.Graphics.DrawString(text, lstTests.Font, b, loc);

				var size = e.Graphics.MeasureString(text, lstTests.Font);

				size.Width += indent + 5;

				if (lstTests.ColumnWidth < (int)size.Width)
					lstTests.ColumnWidth = (int)size.Width;

				if (lstTests.ItemHeight < (int)size.Height)
					lstTests.ItemHeight = (int)size.Height;
			}

			e.DrawFocusRectangle();
		}
		
		private void lstTests_SelectedIndexChanged(object sender, EventArgs e)
		{
			StringBuilder b = new StringBuilder();

			TestInfo test = lstTests.SelectedItem as TestInfo;

			if (test != null)
			{
				var testtype = GetTestType(test.Class);

				b.AppendLine(test.Name);
				if (testtype != null)
					b.AppendLine(testtype.Name);
				else
					b.AppendLine("NO VALID INTERFACE");

				b.AppendLine(test.Class.FullName);
				b.AppendLine(test.Class.Assembly.GetName().Name);
			}

			txtTestInfo.Text = b.ToString();
		}

		Type GetTestType(Type testclass)
		{
			foreach (var type in interfaces)
			{
				if (type.IsAssignableFrom(testclass))
					return type;
			}

			return null;
		}

		private void frmLauncher_Shown(object sender, EventArgs e)
		{
			FillList();
		}

		private void frmLauncher_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
		}
	}
}