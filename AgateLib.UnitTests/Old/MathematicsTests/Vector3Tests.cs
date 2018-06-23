using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class Vector3Tests
	{
		[TestMethod]
		public void V3_DotProductOrtho()
		{
			Vector3 v1 = Vector3.UnitX + 2 * Vector3.UnitZ;
			Vector3 v2 = Vector3.UnitY * 4;

			Assert.AreEqual(0, v1.DotProduct(v2));
		}

		[TestMethod]
		public void V3_Magnitude()
		{
			Vector3 v = new Vector3(3, 4, 12);

			Assert.AreEqual(13, v.Magnitude, 0.00001);
		}
	}
}
