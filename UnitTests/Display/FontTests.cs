using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.UnitTests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Display
{
	[TestClass]
	public class FontTests
	{
		Font ff;

		[TestInitialize]
		public void Init()
		{
			ff = new Font("times");

			ff.AddFont(new FontSettings(8, FontStyle.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(8, FontStyle.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(10, FontStyle.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 10 }));

			ff.AddFont(new FontSettings(10, FontStyle.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 10 }));
		}
		[TestCleanup]
		public void Terminate()
		{
		}

		[TestMethod]
		public void FFBasicRetrieval()
		{
			Assert.AreEqual(new FontSettings(8, FontStyle.None), ff.GetClosestFontSettings(new FontSettings(8, FontStyle.None)));
			Assert.AreEqual(new FontSettings(10, FontStyle.None), ff.GetClosestFontSettings(new FontSettings(10, FontStyle.None)));

			Assert.AreEqual(new FontSettings(8, FontStyle.None), ff.GetClosestFontSettings(new FontSettings(7, FontStyle.None)));
			Assert.AreEqual(new FontSettings(10, FontStyle.None), ff.GetClosestFontSettings(new FontSettings(9, FontStyle.None)));
			Assert.AreEqual(new FontSettings(10, FontStyle.None), ff.GetClosestFontSettings(new FontSettings(11, FontStyle.None)));
		}

		[TestMethod]
		public void FFAutoScale()
		{
			var font = ff.GetClosestFont(9, FontStyle.None);

			Assert.AreEqual(0.9, font.ScaleHeight, 0.00001);
		}

		[TestMethod]
		public void FFRemoveStyleRetrieval()
		{
			Assert.AreEqual(new FontSettings(10, FontStyle.None), ff.GetClosestFontSettings(new FontSettings(9, FontStyle.Italic)));
			Assert.AreEqual(new FontSettings(10, FontStyle.Bold), ff.GetClosestFontSettings(new FontSettings(9, FontStyle.Italic | FontStyle.Bold)));

		}
	}
}
