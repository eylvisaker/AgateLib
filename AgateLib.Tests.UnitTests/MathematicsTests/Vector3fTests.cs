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
	public class Vector3fTests
	{
		[TestMethod]
		public void V3f_DotProductOrtho()
		{
			Vector3f v1 = Vector3f.UnitX + 2 * Vector3f.UnitZ;
			Vector3f v2 = Vector3f.UnitY * 4;

			Assert.AreEqual(0, v1.DotProduct(v2));
		}

		[TestMethod]
		public void V3f_Magnitude()
		{
			Vector3f v = new Vector3f(3, 4, 12);

			Assert.AreEqual(13, v.Magnitude, 0.00001);
		}
	}
}
