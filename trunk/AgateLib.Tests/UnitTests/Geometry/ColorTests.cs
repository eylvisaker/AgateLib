using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Geometry
{
	[TestClass]
	public class ColorTests
	{
		[TestMethod]
		public void NamedColors()
		{
			Assert.IsTrue(Color.IsNamedColor("Red"));
			Assert.IsTrue(Color.IsNamedColor("Green"));
			Assert.IsTrue(Color.IsNamedColor("Blue"));
			Assert.IsFalse(Color.IsNamedColor("Atari"));

			Assert.AreEqual(Color.FromArgb(255, 255, 0, 0), Color.Red);
		}

		[TestMethod]
		public void ColorValues()
		{
			Color test = Color.FromArgb(0x22, 0x33, 0x44, 0x55);

			Assert.AreEqual(0x22, test.A);
			Assert.AreEqual(0x33, test.R);
			Assert.AreEqual(0x44, test.G);
			Assert.AreEqual(0x55, test.B);

			Assert.AreEqual(0x22334455, test.ToArgb());
			Assert.AreEqual(0x22554433, test.ToAbgr());
		}
	}
}
