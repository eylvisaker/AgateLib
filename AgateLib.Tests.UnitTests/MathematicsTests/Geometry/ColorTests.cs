using AgateLib.DisplayLib;
using AgateLib.Quality;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.MathematicsTests.Geometry
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

			Assert.AreEqual(Color.Red, Color.GetNamedColor("Red"));
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

		[TestMethod]
		public void FromArgb()
		{
			var clr = Color.FromArgb(0x11223344);
			var value = clr.ToArgb();

			Assert.AreEqual(0x11223344, value);
			Assert.AreEqual(0x11, clr.A);
			Assert.AreEqual(0x22, clr.R);
			Assert.AreEqual(0x33, clr.G);
			Assert.AreEqual(0x44, clr.B);
		}

		[TestMethod]
		public void HsvRed()
		{
			var clr1 = Color.FromHsv(0, 1, 1);
			var clr2 = Color.FromHsv(360, 1, 1);

			Assert.AreEqual(255, clr1.A);
			Assert.AreEqual(255, clr1.R);
			Assert.AreEqual(0, clr1.G);
			Assert.AreEqual(0, clr1.B);
			Assert.AreEqual(0xffff0000, (uint)clr1.ToArgb());

			Assert.AreEqual(clr1, clr2);
		}

		[TestMethod]
		public void HsvYellow()
		{
			var clr1 = Color.FromHsv(60, 1, 1);
			var clr2 = Color.FromHsv(420, 1, 1);

			Assert.AreEqual(0xffffff00, (uint)clr1.ToArgb());
			Assert.AreEqual(clr1, clr2);
		}

		[TestMethod]
		public void HsvGreen()
		{
			var clr1 = Color.FromHsv(120, 1, 1);
			var clr2 = Color.FromHsv(-240, 1, 1);

			Assert.AreEqual(0xff00ff00, (uint)clr1.ToArgb());
			Assert.AreEqual(clr1, clr2);
		}

		[TestMethod]
		public void HsvBlue()
		{
			var clr1 = Color.FromHsv(240, 1, 1);

			Assert.AreEqual(0xff0000ff, (uint)clr1.ToArgb());
		}

		[TestMethod]
		public void HsvCyan()
		{
			var clr1 = Color.FromHsv(200, 1, 1);

			Assert.AreEqual(0xff00a9ff, (uint)clr1.ToArgb());
		}

		[TestMethod]
		public void HsvMagenta()
		{
			var clr1 = Color.FromHsv(330, 1, 1);

			Assert.AreEqual(0xffff007f, (uint)clr1.ToArgb());
		}

		[TestMethod]
		public void FromArgbString()
		{
			AssertX.Throws(() => Color.FromArgb("lkdsjflkdsj"));

			var clr = Color.FromArgb("0xaabbcc");

			Assert.AreEqual(0xff, clr.A);
			Assert.AreEqual(0xaa, clr.R);
			Assert.AreEqual(0xbb, clr.G);
			Assert.AreEqual(0xcc, clr.B);

			var clr1 = Color.FromArgb("0x99aabbcc");

			Assert.AreEqual(0x99, clr1.A);
			Assert.AreEqual(0xaa, clr1.R);
			Assert.AreEqual(0xbb, clr1.G);
			Assert.AreEqual(0xcc, clr1.B);
		}

		[TestMethod]
		public void ToArgbString()
		{
			string[] colorsToTest = new[]
			{
				"ffaabbcc", "20f8f8f8", "ffaabbcc",
				"ffffffff", "01000000", "01020304"
			};

			foreach (var color in colorsToTest)
			{
				var text = Color.FromArgb(color).ToArgbString();
				Assert.AreEqual(color, text.ToLowerInvariant(),
					"Failed to convert color {0} correctly.", color);
			}

		}
	}

}
