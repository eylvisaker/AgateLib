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
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing
{
	public partial class frmLauncher : Form, AgateLib.Settings.ISettingsTracer
	{

		IList<TestInfo> tests { get { return TestCollection.Tests; } }

		Font bold;

		public frmLauncher()
		{
			InitializeComponent();

			Icon = AgateLib.Platform.WinForms.Controls.FormUtil.AgateLibIcon;

			LoadTests();

			bold = new Font(lstTests.Font, FontStyle.Bold | FontStyle.Italic);

			ReadSettingsNames();

			AgateLib.Settings.PersistantSettings.SettingsTracer = this;
			AgateLib.Settings.PersistantSettings.Debug = true;

			this.FormClosed += HandleFormClosed;

			ResetCommandLine();
		}

		private void frmLauncher_Load(object sender, EventArgs e)
		{
		}

		void HandleFormClosed(object sender, FormClosedEventArgs e)
		{
			if (settingsFile == null)
			{
				System.Diagnostics.Debug.Print("No settings file to save to.");
				return;
			}

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
			if (string.IsNullOrEmpty(groupName))
				throw new ArgumentException();

			string name = groupName + "." + key;

			StoreSetting(name, value);
		}

		void AgateLib.Settings.ISettingsTracer.OnWriteSetting(string groupName, string key, string value)
		{
			if (string.IsNullOrEmpty(groupName))
				throw new ArgumentException();

			string name = groupName + "." + key;

			StoreSetting(name, value);
		}

		void ReadSettingsNames()
		{
			//StreamReader r = null;
			//string targetDirectory = "../../../Tests/Assets/";
			//string filename = "settings_list.txt";

			//try
			//{
			//	settingsFile = System.IO.Path.GetFullPath(targetDirectory + filename);
			//	r = new StreamReader(targetDirectory + filename);
			//}
			//catch (DirectoryNotFoundException)
			//{
			//	settingsFile = filename;
			//	r = new StreamReader(filename);
			//}

			//using (r)
			//{
			//	while (r.EndOfStream == false)
			//	{
			//		string x = r.ReadLine().Trim();

			//		if (string.IsNullOrEmpty(x))
			//			continue;

			//		int index = x.IndexOf('\t');

			//		if (index == -1)
			//		{
			//			mSettings[x] = null;
			//		}
			//		else
			//			mSettings[x.Substring(0, index)] = x.Substring(index + 1);
			//	}
			//}
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
			TestCollection.AddTests(Assembly.GetAssembly(GetType()));

			FillList();
		}


		private void AddTest(Type t)
		{
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
			foreach (var kvp in mSettings.ToArray())
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
				LaunchTestModel(obj);
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

		private void LaunchTestModel(IAgateTest test)
		{
			try
			{
				if (test is ISerialModelTest) LaunchTestModel((ISerialModelTest)test);
				else if (test is ISceneModelTest) LaunchTestModel((ISceneModelTest)test);
				else if (test is IDiscreteAgateTest) LaunchTestModel((IDiscreteAgateTest)test);
				else
					MessageBox.Show(this, "The test " + test.Name + " does not have a model defined.", "AgateLib Test can't run", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch (ExitGameException)
			{ }
		}
		private void LaunchTestModel(ISerialModelTest test)
		{
			var parameters = CreateParameters<SerialModelParameters>(test);
			test.ModifyModelParameters(parameters);

			using (var model = new SerialModel(parameters))
			{
				model.Run(new Action(test.EntryPoint));
			}
		}
		private void LaunchTestModel(ISceneModelTest test)
		{
			var parameters = CreateParameters<SceneModelParameters>(test);
			test.ModifyModelParameters(parameters);

			using (var model = new SceneModel(parameters))
			{
				model.Run(test.StartScene);
			}
		}
		private void LaunchTestModel(IDiscreteAgateTest test)
		{
			test.Main(CommandLineArguments);
		}

		private T CreateParameters<T>(IAgateTest test) where T : ModelParameters, new()
		{
			var parameters = new T();

			parameters.Arguments = CommandLineArguments;
			parameters.AssetLocations.Path = "Assets";
			parameters.AssetLocations.UserInterface = "UserInterface";
			parameters.ApplicationName = test.Name + " :: " + test.Category + " test";

			return parameters;
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
				string text = t.Name;

				if (GetTestType(t.Class) == null)
					text += " *INVALID";

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

		readonly char[] separator = new char[] { ' ' };

		private void txtCommandLine_TextChanged(object sender, EventArgs e)
		{
			ResetCommandLine();
		}

		private void ResetCommandLine()
		{
			CommandLineArguments = txtCommandLine.Text.Split(separator,
						 StringSplitOptions.RemoveEmptyEntries);
		}

		string[] CommandLineArguments { get; set; }
		Type[] interfaces = new Type[] { typeof(ISceneModelTest), typeof(ISerialModelTest), typeof(IDiscreteAgateTest) };

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
	}
}