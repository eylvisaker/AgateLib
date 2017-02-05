using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.DisplayLib
{
	[TestClass]
	public class LayoutTextTests : AgateUnitTest
	{
		private IFont font;

		[TestInitialize]
		public void Initialize()
		{
			font = Font.AgateMono;
		}

		[TestMethod]
		public void TwoLineLayout()
		{
			var layout = font.LayoutText("This is a test of wrapped two line text.", 120);

			Assert.AreEqual(20, layout.Height);
		}
	}
}
