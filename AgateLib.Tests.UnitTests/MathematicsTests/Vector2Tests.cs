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
	public class Vector2Tests
	{
		[TestMethod]
		public void V2_DotProductOrtho()
		{
			Vector2 v1 = Vector2.Xhat;
			Vector2 v2 = Vector2.Yhat * 4;

			Assert.AreEqual(0, v1.DotProduct(v2));
		}
	}
}
