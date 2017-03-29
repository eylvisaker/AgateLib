using System.Collections.Generic;
using AgateLib.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Collections
{
	[TestClass]
	public class ListExtensions
	{
		[TestMethod]
		public void SortPrimitives()
		{
			List<int> li = new List<int> { 1, 6, 2, 3, 8, 10, 9, 7, 4, 5 };

			Assert.AreEqual(10, li.Count);

			li.InsertionSort();

			for (int i = 0; i < li.Count; i++)
				Assert.AreEqual(i + 1, li[i]);
		}

		[TestMethod]
		public void InsertionSortTest()
		{
			List<int> list = new List<int> { 4, 2, 3, 1, 6, 7, 8, 9 };

			list.InsertionSort();

			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(9, list[list.Count - 1]);
		}
	}
}
