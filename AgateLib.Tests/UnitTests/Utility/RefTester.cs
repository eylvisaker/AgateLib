using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.Utility
{
	[TestClass]
	public class RefTester
	{
		[TestMethod]
		public void RefEqualsTest()
		{
			var a = new RefTest();
			var b = new RefTest();

			var ra = new Ref<RefTest>(a);
			var rb = new Ref<RefTest>(b);

			Assert.IsFalse(ra.Equals(rb));
			Assert.IsTrue(ra.Equals(ra));

			object ora = ra;
			object orb = rb;

			Assert.IsFalse(ora.Equals(1));
			Assert.IsFalse(ora.Equals(orb));
			Assert.IsTrue(ora.Equals(ora));

			Assert.IsTrue(ora.Equals(a));
		}
	}
}
