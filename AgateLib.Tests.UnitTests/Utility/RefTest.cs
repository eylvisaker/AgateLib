using System;

using AgateLib.Utility;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Utility
{
	[TestClass]
	public class RefTest : IDisposable
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

			using (Ref<RefTest> r = new Ref<RefTest>(this))
			{
				Assert.AreEqual(1, r.Counter.GetRefCount());

				using (Ref<RefTest> other = new Ref<RefTest>(r))
				{
					Assert.AreEqual(2, r.Counter.GetRefCount());
				}

				Assert.AreEqual(1, r.Counter.GetRefCount());

				// even though the second reference has been disposed, the object
				// has not been disposed until we exit the current using block.
				Assert.AreEqual(0, disposeCount);
			}

			// now the object has been disposed.
			Assert.AreEqual(1, disposeCount);
		}
	}
}
