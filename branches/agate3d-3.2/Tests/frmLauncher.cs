using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Tests
{
	public partial class frmLauncher : Form
	{
		class TestInfo
		{
			public string Name { get; set; }
			public string Category { get; set; }
			public Type Class { get; set; }
		}

		List<TestInfo> tests = new List<TestInfo>();
		Font bold;

		public frmLauncher()
		{
			InitializeComponent();

			Icon = AgateLib.WinForms.FormUtil.AgateLibIcon;

			LoadTests();

			bold = new Font(lstTests.Font, FontStyle.Bold | FontStyle.Italic);
		}

		private void FillList()
		{
			tests.Sort((x, y) =>
			{
				if (x.Class == y.Class)
					return x.Name.CompareTo(y.Name);
				else
					return x.Category.CompareTo(y.Category);
			});

			string lastCategory = null;

			foreach (var test in tests)
			{
				if (test.Category != lastCategory)
				{
					lstTests.Items.Add(test.Category);
					lastCategory = test.Category;
				}

				this.lstTests.Items.Add(test);
			}
		}

		private void LoadTests()
		{
			Assembly myass = Assembly.GetAssembly(typeof(frmLauncher));

			foreach (var t in myass.GetTypes())
			{
				if (typeof(IAgateTest).IsAssignableFrom(t) && t.IsAbstract == false)
				{
					AddTest(t);
				}
			}

			FillList();
		}


		private void AddTest(Type t)
		{
			IAgateTest obj = (IAgateTest)Activator.CreateInstance(t);

			tests.Add(new TestInfo { Name = obj.Name, Category = obj.Category, Class = t });
		}

		private void lstTests_DoubleClick(object sender, EventArgs e)
		{
			TestInfo t = lstTests.SelectedItem as TestInfo;
			if (t == null)
				return;

			LaunchTest(t);
		}

		private void LaunchTest(TestInfo m)
		{
			IAgateTest obj = (IAgateTest)Activator.CreateInstance(m.Class);

			string[] args = { "--choose" };

			this.Hide();

			try
			{
				obj.Main(args);
			}
			catch (TargetInvocationException e)
			{
				Exception ex_relevant = e.InnerException ?? e;
				string info = ex_relevant.Message;

				MessageBox.Show(
					ex_relevant.GetType().Name + Environment.NewLine + info,
					"AgateLib Test " + m.Name + " threw an exception.",
					MessageBoxButtons.OK,
					MessageBoxIcon.Stop);

			}
			finally
			{
				this.Show();
				Cursor.Show();
				this.TopMost = true;
				this.TopMost = false;
				this.Activate();
			}
		}

		private void lstTests_DrawItem(object sender, DrawItemEventArgs e)
		{
			object item = lstTests.Items[e.Index];
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

				e.Graphics.DrawString(t.Name, lstTests.Font, b, loc);

				var size = e.Graphics.MeasureString(t.Name, lstTests.Font);

				size.Width += indent + 5;

				if (lstTests.ColumnWidth < size.Width)
					lstTests.ColumnWidth = (int) size.Width;
			}

			e.DrawFocusRectangle();
		}

		private void lstTests_MeasureItem(object sender, MeasureItemEventArgs e)
		{

		}


	}
}