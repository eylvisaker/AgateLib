using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.Platform.WinForms;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Settings;

namespace AgateLib.Tests
{
	public class TestLauncherPresenter
	{
		private frmLauncher frm;

		private bool runningTest;

		readonly char[] separator = { ' ' };

		public TestLauncherPresenter(frmLauncher frm)
		{
			this.frm = frm;

			frm.LaunchTest += Frm_LaunchTest;

			LoadTests();
		}

		private string[] CommandLineArguments
		{
			get
			{
				return frm.CommandLine.Split(separator,
					  StringSplitOptions.RemoveEmptyEntries);
			}
		}

		private void Frm_LaunchTest(object sender, TestEventArgs e)
		{
			LaunchTest(e.Info);
		}

		private void LaunchTest(TestInfo m)
		{
			IAgateTest obj = (IAgateTest)Activator.CreateInstance(m.Class);

			if (runningTest)
			{
				System.Diagnostics.Debug.Print("Bug in mono? A second test was launched while the first was still running.");
				return;
			}

			string[] args = { };

			frm.HideBeforeTest();

			try
			{
				runningTest = true;
				LaunchTestModel(obj);
			}
			catch (TargetInvocationException e)
			{
				Exception exRelevant = e.InnerException ?? e;
				string info = exRelevant.Message;

				frm.TestExceptionMessage($"{exRelevant.GetType().Name}\n{info}", $"AgateLib Test {m.Name} threw an exception.");

			}
			finally
			{
				runningTest = false;
				frm.ShowAfterTest();
			}
		}

		private void LaunchTestModel(IAgateTest test)
		{
			using (new AgateWinForms(CommandLineArguments)
				.AssetPath("Assets")
				.Initialize()
				.InstallConsoleCommands())
			{
				test.Run(CommandLineArguments);
			}

			Application.DoEvents();
			GC.Collect();
		}

		private void SplitName(string p, out string group, out string key)
		{
			int period = p.LastIndexOf('.');

			if (period == -1)
				throw new DataException("Invalid key name");

			group = p.Substring(0, period);
			key = p.Substring(period + 1);
		}

		private void LoadTests()
		{
			TestCollection.AddTests(Assembly.GetAssembly(GetType()));

			frm.Tests = TestCollection.Tests;
		}
	}
}
