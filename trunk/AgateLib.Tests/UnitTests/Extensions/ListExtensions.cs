using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AgateLib.Extensions.Collections.Generic;

namespace AgateLib.Extensions
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
	}
}
