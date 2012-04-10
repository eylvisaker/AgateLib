using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Tests
{
	public partial class frmLauncher : Form, AgateLib.Settings.ISettingsTracer
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

			FillDrivers();

			ReadSettingsNames();

			AgateLib.Settings.PersistantSettings.SettingsTracer = this;
			AgateLib.Settings.PersistantSettings.Debug = true;

			this.FormClosed += HandleFormClosed;
		}

		private void frmLauncher_Load(object sender, EventArgs e)
		{
		}

		private void FillDrivers()
		{
			AgateLib.Core.Initialize();

			AddDrivers(displayList, AgateLib.Drivers.Registrar.DisplayDrivers);
			AddDrivers(audioList, AgateLib.Drivers.Registrar.AudioDrivers);
			AddDrivers(inputList, AgateLib.Drivers.Registrar.InputDrivers);
		}

		private void AddDrivers(ComboBox list, List<AgateLib.Drivers.AgateDriverInfo> drivers)
		{
			list.Items.Clear();
			list.Items.Add("Default");

			foreach (var driver in drivers)
			{
				list.Items.Add(driver.FriendlyName);
			}

			list.SelectedIndex = 0;
		}
		void HandleFormClosed(object sender, FormClosedEventArgs e)
		{
			using (StreamWriter w = new StreamWriter(settingsFile))
			{
				foreach (var setting in mSettings)
				{
					string text = setting.Key + "\t" + setting.Value;

					System.Diagnostics.Debug.Print(text);
					w.WriteLine(text);
				}
			}
		}

		#region --- ISettingsTracer implementation ---

		Dictionary<string, string> mSettings = new Dictionary<string, string>();
		string settingsFile;

		void AgateLib.Settings.ISettingsTracer.OnReadSetting(string groupName, string key, string value)
		{
			string name = groupName + "." + key;

			StoreSetting(name, value);
		}

		void AgateLib.Settings.ISettingsTracer.OnWriteSetting(string groupName, string key, string value)
		{
			string name = groupName + "." + key;

			StoreSetting(name, value);
		}

		void ReadSettingsNames()
		{
			StreamReader r = null;
			string targetDirectory = "../../Tests/";
			string filename = "settings_list.txt";

			try
			{
				settingsFile = targetDirectory + filename;
				r = new StreamReader(targetDirectory + filename);
			}
			catch (DirectoryNotFoundException)
			{
				settingsFile = filename;
				r = new StreamReader(filename);
			}

			using (r)
			{
				while (r.EndOfStream == false)
				{
					string x = r.ReadLine().Trim();

					if (string.IsNullOrWhiteSpace(x))
						continue;

					int index = x.IndexOf('\t');

					if (index == -1)
					{
						mSettings[x] = null;
					}
					else
						mSettings[x.Substring(0, index)] = x.Substring(index + 1);
				}
			}

			displayList.SelectedItem = mSettings["AgateLib.DisplayDriver"];
			audioList.SelectedItem = mSettings["AgateLib.AudioDriver"];
			inputList.SelectedItem = mSettings["AgateLib.InputDriver"];
		}
		void StoreSetting(string name, string value)
		{
			if (mSettings.ContainsKey(name)) return;

			System.Diagnostics.Debug.Print("Storing setting " + name);

			mSettings.Add(name, value);
		}

		#endregion

		private void FillList()
		{
			tests.Sort((x, y) =>
			{
				if (x.Category != y.Category)
					return x.Category.CompareTo(y.Category);
				else
					return x.Name.CompareTo(y.Name);
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

		private void lstTests_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				TestInfo t = lstTests.SelectedItem as TestInfo;
				if (t == null)
					return;

				LaunchTest(t);
			}
		}

		bool runningTest = false;

		private void LaunchTest(TestInfo m)
		{
			IAgateTest obj = (IAgateTest)Activator.CreateInstance(m.Class);
			if (runningTest)
			{
				System.Diagnostics.Debug.Print("Bug in mono? A second test was launched while the first was still running.");
				return;
			}


			string[] args = { };

			this.Hide();
			foreach (var kvp in mSettings)
			{
				if (kvp.Value == null)
					continue;

				string group, key;
				SplitName(kvp.Key, out group, out key);

				AgateLib.Core.Settings[group][key] = kvp.Value;
			}

			try
			{
				runningTest = true;
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
				runningTest = false;
				this.Show();
				Cursor.Show();
				this.TopMost = true;
				this.TopMost = false;
				this.Activate();
			}
		}

		private void SplitName(string p, out string group, out string key)
		{
			int period = p.LastIndexOf('.');

			if (period == -1)
				throw new DataException("Invalid key name");

			group = p.Substring(0, period);
			key = p.Substring(period + 1);

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
					lstTests.ColumnWidth = (int)size.Width;
			}

			e.DrawFocusRectangle();
		}

		private void displayList_SelectedIndexChanged(object sender, EventArgs e)
		{
			mSettings["AgateLib.DisplayDriver"] = displayList.Text;
		}
		private void audioList_SelectedIndexChanged(object sender, EventArgs e)
		{
			mSettings["AgateLib.AudioDriver"] = audioList.Text;
		}
		private void inputList_SelectedIndexChanged(object sender, EventArgs e)
		{
			mSettings["AgateLib.InputDriver"] = inputList.Text;
		}

	}
}