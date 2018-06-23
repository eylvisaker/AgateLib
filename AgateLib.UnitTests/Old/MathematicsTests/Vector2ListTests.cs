using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class Vector2ListTests
	{

		[TestMethod]
		public void V2List_InsertRange()
		{
			Vector2List list = new Vector2List
			{
				{ 0, 0 },
				{ 1, 1 },
				{ 2, 2 },
				{ 8, 8 },
				{ 9, 9 }
			};

			Vector2List secondList = new Vector2List
			{
				{ 3, 3 },
				{ 4, 4 },
				{ 5, 5 },
				{ 6, 6 },
				{ 7, 7 }
			};

			list.InsertRange(3, secondList);

			Assert.AreEqual(10, list.Count);

			for (int i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(i, list[i].X);
				Assert.AreEqual(i, list[i].Y);
			}
		}
	}
}
