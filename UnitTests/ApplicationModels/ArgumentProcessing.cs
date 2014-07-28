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
	public 	class ArgumentProcessing : AgateAppModel
	{
		public ArgumentProcessing(ModelParameters param) : base(param)
		{

		}
		protected override int BeginModel(Func<int> entryPoint)
		{
			return 0;
		}

		protected override void Initialize()
		{
		}

		protected override void Dispose()
		{
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

			Assert.IsFalse(p.Parameters.CreateFullScreenWindow);
			Assert.AreEqual(new Size(640, 480), p.Parameters.DisplayWindowSize);
		}
	}
}
