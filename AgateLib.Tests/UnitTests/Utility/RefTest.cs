﻿using AgateLib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Utility
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

				// even though the second reference has been disposed, the object
				// has not been disposed until we exit the current using block.
				Assert.AreEqual(0, disposeCount);
			}

			// now the object has been disposed.
			Assert.AreEqual(1, disposeCount);
		}

	}
}