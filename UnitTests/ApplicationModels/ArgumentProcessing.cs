using AgateLib.ApplicationModels;
using AgateLib.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.ApplicationModels
{
	public class ArgumentProcessing : AgateAppModel
	{
		List<string> expected = new List<string>();

		public ArgumentProcessing(ModelParameters param)
			: base(param)
		{

		}
		protected override int BeginModel(Func<int> entryPoint)
		{
			return 0;
		}

		public List<string> Expected { get { return expected; } }

		protected override void InitializeImpl()
		{
		}

		public override void KeepAlive()
		{
		}

		protected override void ProcessArgument(string arg, string parm)
		{
			if (Expected.Count > 0)
			{
				Assert.AreEqual(expected[0], arg);
				Assert.IsTrue(arg.StartsWith("--"));

				expected.RemoveAt(0);

				if (parm != "")
				{
					Assert.AreEqual(expected[0], parm);
					expected.RemoveAt(0);
				}
			}

			base.ProcessArgument(arg, parm);
		}
	}

	[TestClass]
	public class ArgumentProcessorTest
	{
		[TestMethod]
		public void SetWindowSizeTest()
		{
			ArgumentProcessing p = new ArgumentProcessing(new ModelParameters
			{
				Arguments =
					"--window 640x480".Split(' ')
			});

			p.Initialize();

			Assert.IsFalse(p.Parameters.CreateFullScreenWindow);
			Assert.AreEqual(new Size(640, 480), p.Parameters.DisplayWindowSize);
		}

		[TestMethod]
		public void ExtraArguments()
		{
			ArgumentProcessing p = new ArgumentProcessing(new ModelParameters
			{
				Arguments =
					"--window 640x480 --something --else 14 --nothing".Split(' ')
			});

			p.Expected.Add("--window");
			p.Expected.Add("640x480");
			p.Expected.Add("--something");
			p.Expected.Add("--else");
			p.Expected.Add("14");
			p.Expected.Add("--nothing");

			p.Initialize();

			Assert.IsFalse(p.Parameters.CreateFullScreenWindow);
			Assert.AreEqual(new Size(640, 480), p.Parameters.DisplayWindowSize);
		}
	}
}
