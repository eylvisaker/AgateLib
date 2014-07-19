using AgateLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utility
{
	[TestClass]
	public class RefTester : IDisposable
	{
		int disposeCount = 0;

		void IDisposable.Dispose()
		{
			disposeCount++;
		}

		[TestMethod]
		public void ReferenceCountTest()
		{
			disposeCount = 0;

			using (Ref<RefTester> r = new Ref<RefTester>(this))
			{
				Assert.AreEqual(1, r.Counter.GetRefCount());

				using (Ref<RefTester> other = new Ref<RefTester>(r))
				{
					Assert.AreEqual(2, r.Counter.GetRefCount());
				}

				Assert.AreEqual(1, r.Counter.GetRefCount());
				Assert.AreEqual(0, disposeCount);
			}

			Assert.AreEqual(1, disposeCount);
		}
	}
}
