using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RigidBodyDynamics;

namespace AgateLib.Physics.UnitTests
{
	[TestClass]
	public class ValueHistoryTests
	{
		[TestMethod]
		public void HistorySizeTwo()
		{
			ValueHistory<int> history = new ValueHistory<int> {Size = 2};

			history.Current = 1;
			Assert.AreEqual(1, history.Current);

			history.Cycle();
			Assert.AreEqual(0, history.Current);
			Assert.AreEqual(1, history[1]);

			history.Current = 2;
			history.Cycle();

			Assert.AreEqual(1, history.Current);
			Assert.AreEqual(2, history[1]);
		}
	}
}
