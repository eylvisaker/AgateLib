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
			public Type Class { get; set; }
		}

		Dictionary<string, List<TestInfo>> tests = new Dictionary<string, List<TestInfo>>();


		public frmLauncher()
		{
			InitializeComponent();

			Icon = AgateLib.WinForms.FormUtil.AgateLibIcon;

			LoadTests();
		}

		private void FillList()
		{
			foreach (string key in tests.Keys)
			{
				TreeNode n = new TreeNode { Text = key };
				int index = tree.Nodes.Add(n);

				AddChildren(tests[key], index);

				n.Expand();
			}

			tree.Sort();
		}

		private void AddChildren(List<TestInfo> tests, int nodeIndex)
		{
			foreach (var test in tests)
			{
				tree.Nodes[nodeIndex].Nodes.Add(new TreeNode { Text = test.Name, Tag = test });
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

			if (tests.ContainsKey(obj.Category) == false)
			{
				tests[obj.Category] = new List<TestInfo>();
			}

			tests[obj.Category].Add(new TestInfo { Name = obj.Name, Class = t });
		}

		private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			TestInfo t = e.Node.Tag as TestInfo;
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
				this.TopMost = true;
				this.TopMost = false;
				this.Activate();
			}
		}
	}
}