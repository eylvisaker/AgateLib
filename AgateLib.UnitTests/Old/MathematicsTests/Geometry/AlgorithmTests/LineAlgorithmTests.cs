using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests.Geometry.AlgorithmTests
{
	[TestClass]
	public class LineAlgorithmTests
	{
		[TestMethod]
		public void Line_SideOf()
		{
			Assert.AreEqual(-1, LineAlgorithms.SideOf(
				Vector2.Zero, Vector2.UnitX, Vector2.UnitY));
		
			Assert.AreEqual(1, LineAlgorithms.SideOf(
				Vector2.Zero, Vector2.UnitX, -Vector2.UnitY));
		}
	}
}
