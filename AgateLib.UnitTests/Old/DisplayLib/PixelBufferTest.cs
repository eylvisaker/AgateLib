using System;
using System.Runtime.InteropServices;

using AgateLib.DisplayLib;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;
using AgateLib.UnitTests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.DisplayLib
{
	[TestClass]
	public class PixelBufferTest : AgateUnitTest
	{
		readonly PixelFormat[] FullFormats =
			{
				PixelFormat.BGRA8888, PixelFormat.RGBA8888, PixelFormat.ARGB8888, PixelFormat.ABGR8888,
			};

		PixelBuffer src;

		[TestInitialize]
		public void Init()
		{
			src = new PixelBuffer(PixelFormat.ABGR8888, new Size(30, 20));

			for (int j = 0; j < src.Height; j++)
			{
				for (int i = 0; i < src.Width; i++)
				{
					src.SetPixel(i, j, SourceColorAt(i, j));
				}
			}
		}

		private static Color SourceColorAt(int x, int y)
		{
			return Color.FromRgb(255 - x * 3, 255 - y * 4, 255 - x * 2 - y * 2);
		}

		private void VerifyCopyResult(PixelBuffer result)
		{
			VerifyCopyResult(result, src, Point.Zero);
		}
		private void VerifyCopyResult(PixelBuffer result, PixelBuffer srcBuffer)
		{
			VerifyCopyResult(result, srcBuffer, Point.Zero);
		}
		private void VerifyCopyResult(PixelBuffer result, PixelBuffer srcBuffer, Point destPt)
		{
			for (int j = 0; j < srcBuffer.Height; j++)
			{
				for (int i = 0; i < srcBuffer.Width; i++)
				{
					int x = i + destPt.X;
					int y = j + destPt.Y;

					var resultPx = result.GetPixel(x, y);
					var srcPx = srcBuffer.GetPixel(i, j);

					if (resultPx.A == 0 && srcPx.A == 0)
						continue;

					Assert.AreEqual(srcPx, resultPx);
				}
			}
		}

		[TestMethod]
		public void CopyPixels()
		{
			var result = new PixelBuffer(PixelFormat.ABGR8888, new Size(40, 50));
			var destPt = new Point(5, 5);

			result.CopyFrom(src, new Rectangle(0, 0, 30, 20), destPt, true);

			VerifyCopyResult(result, src, destPt);
		}

		[TestMethod]
		public void CopyFromInvalidArgs()
		{
			var result = new PixelBuffer(PixelFormat.ABGR8888, new Size(40, 50));

			AssertX.Throws<ArgumentNullException>(() => result.CopyFrom(null, new Rectangle(), Point.Zero, true));
			AssertX.Throws<ArgumentOutOfRangeException>(() => result.CopyFrom(src, new Rectangle(-4, -4, 4, 4), Point.Zero, true));
			AssertX.Throws<ArgumentOutOfRangeException>(() => result.CopyFrom(src, new Rectangle(0, 0, 4, 4), new Point(-4, -4), true));
			AssertX.Throws<ArgumentException>(() => result.CopyFrom(src, new Rectangle(0, 0, 30, 20), new Point(39, 49), false));
			AssertX.Throws<ArgumentException>(() => result.CopyFrom(src, new Rectangle(0, 0, 30, 20), new Point(0, 49), false));
		}

		[TestMethod]
		public void CopyFromDifferentFormat()
		{
			var result = new PixelBuffer(PixelFormat.ARGB8888, new Size(30, 20));

			result.CopyFrom(src, new Rectangle(Point.Zero, src.Size), Point.Zero, false);

			VerifyCopyResult(result);
		}


		[TestMethod]
		public void CopyFromSkipTransparent()
		{
			var result = new PixelBuffer(PixelFormat.ARGB8888, new Size(30, 20));

			src.SetPixel(0, 0, Color.FromArgb(0, 255, 255, 255));

			result.CopyFrom(src, new Rectangle(Point.Zero, src.Size), Point.Zero, false, true);

			VerifyCopyResult(result);
		}


		[TestMethod]
		public void PixelsEqualDifferentFormat()
		{
			var result = new PixelBuffer(PixelFormat.ARGB8888, src.Size);

			result.CopyFrom(src, new Rectangle(Point.Zero, src.Size), Point.Zero, false);

			VerifyCopyResult(result);

			Assert.IsTrue(PixelBuffer.PixelsEqual(result, src));

			result.SetPixel(0, 0, Color.Black);

			Assert.IsFalse(PixelBuffer.PixelsEqual(src, result));
		}

		[TestMethod]
		public void PixelsEqualSameFormat()
		{
			var result = new PixelBuffer(PixelFormat.ABGR8888, src.Size);

			result.CopyFrom(src, new Rectangle(Point.Zero, src.Size), Point.Zero, false);

			VerifyCopyResult(result);

			Assert.IsTrue(PixelBuffer.PixelsEqual(result, src));

			result.SetPixel(0, 0, Color.Black);

			Assert.IsFalse(PixelBuffer.PixelsEqual(src, result));
		}

		[TestMethod]
		public void PixelsEqualEasyCases()
		{
			Assert.IsFalse(PixelBuffer.PixelsEqual(src, new PixelBuffer(src.PixelFormat, new Size(src.Width, src.Height + 5))));
			Assert.IsFalse(PixelBuffer.PixelsEqual(src, new PixelBuffer(src.PixelFormat, new Size(src.Width, src.Height + 5))));
			Assert.IsTrue(PixelBuffer.PixelsEqual(src, src));
		}

		[TestMethod]
		public void ConvertFormat()
		{
			var result = src.ConvertTo(PixelFormat.RGBA8888);
			Assert.IsTrue(PixelBuffer.PixelsEqual(result, src));

			result = src.ConvertTo(src.PixelFormat);
			Assert.IsTrue(PixelBuffer.PixelsEqual(result, src));
		}

		[TestMethod]
		public void ConvertFormatAndChangeSize()
		{
			var result = src.ConvertTo(PixelFormat.RGBA8888, new Size(50, 50));
			result = new PixelBuffer(result, new Rectangle(Point.Zero, src.Size));
			Assert.IsTrue(PixelBuffer.PixelsEqual(result, src));

			result = src.ConvertTo(src.PixelFormat, new Size(50, 50));
			result = new PixelBuffer(result, new Rectangle(Point.Zero, src.Size));
			Assert.IsTrue(PixelBuffer.PixelsEqual(result, src));
		}

		[TestMethod]
		public void IsRowBlank()
		{
			Assert.IsFalse(src.IsBlank(0.05));

			for (int j = 0; j < src.Height; j++)
			{
				for (int i = 0; i < src.Width; i++)
				{
					var clr = src.GetPixel(i, j);

					src.SetPixel(i, j, Color.FromArgb(0, clr.R, clr.G, clr.B));
				}
			}

			Assert.IsTrue(src.IsBlank(0.05));
		}

		[TestMethod]
		public void IsColumnBlank()
		{
			const int x = 4;
			Assert.IsFalse(src.IsColumnBlank(x, 0.05));

			for (int j = 0; j < src.Height; j++)
			{
				var clr = src.GetPixel(x, j);

				src.SetPixel(x, j, Color.FromArgb(0, clr.R, clr.G, clr.B));
			}

			Assert.IsTrue(src.IsColumnBlank(x, 0.05));
		}

		[TestMethod]
		public void IsRegionBlank()
		{
			var region = new Rectangle(5, 5, 5, 5);
			Assert.IsFalse(src.IsRegionBlank(region, 0.05));

			for (int j = region.Top; j < region.Bottom; j++)
			{
				for (int i = region.Left; i < region.Right; i++)
				{
					var clr = src.GetPixel(i, j);

					src.SetPixel(i, j, Color.FromArgb(0, clr.R, clr.G, clr.B));
				}
			}

			Assert.IsTrue(src.IsRegionBlank(region, 0.05));
			Assert.IsFalse(src.IsBlank(0.05));
		}

		[TestMethod]
		public void ConvertPixels()
		{
			for (int j = 0; j < FullFormats.Length; j++)
			{
				var thisSrc = src.ConvertTo(FullFormats[j]);
				VerifyCopyResult(thisSrc);

				for (int i = 0; i < FullFormats.Length; i++)
				{
					var result = thisSrc.ConvertTo(FullFormats[i]);
					VerifyCopyResult(result);
				}
			}
		}

		[TestMethod]
		public void ReplaceColor()
		{
			var result = src.ConvertTo(src.PixelFormat);

			result.ReplaceColor(Color.White, Color.Black);

			Assert.IsFalse(PixelBuffer.PixelsEqual(src, result));
			Assert.AreEqual(Color.Black, result.GetPixel(0, 0));

			result.SetPixel(0, 0, Color.White);
			Assert.IsTrue(PixelBuffer.PixelsEqual(src, result));
		}

		[TestMethod]
		public void Clear()
		{
			src.Clear(Color.FromArgb(0, 0, 0, 0));

			Assert.IsTrue(src.IsBlank(0.05));
		}

		[TestMethod]
		public void SetPixel()
		{
			var result = src.ConvertTo(PixelFormat.BGRA8888);

			result.SetPixel(0, 0, Color.Red);
			src.SetPixel(0, 0, Color.Red);

			VerifyCopyResult(result);
		}

		[TestMethod]
		public void Clone()
		{
			var result = src.Clone();

			VerifyCopyResult(result);

			Assert.AreNotSame(result, src);
			Assert.AreNotSame(result.Data, src.Data);
		}

	}
}
