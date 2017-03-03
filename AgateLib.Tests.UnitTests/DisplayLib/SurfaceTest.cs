using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.IO;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test;
using AgateLib.Platform.Test.Display;
using AgateLib.Quality;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.DisplayLib
{
	[TestClass]
	public class SurfaceTest : AgateUnitTest
	{
		Surface surface;
		FakeSurfaceImpl surfaceImpl;

		[TestInitialize]
		public void Init()
		{
			surfaceImpl = new FakeSurfaceImpl(new Size(34, 30));
			surface = new Surface(surfaceImpl);

			var pb = surfaceImpl.ReadPixels(PixelFormat.ABGR8888);
			for (int j = 0; j < pb.Height; j++)
			{
				for (int i = 0; i < pb.Width; i++)
				{
					pb.SetPixel(i, j, Color.FromRgb(255 - i, 255 - j, 128));
				}
			}
		}

		[TestMethod]
		public void DrawDestPt()
		{
			surface.Draw(new Point(44, 33));
			var draw = surfaceImpl.LastDraw.DrawInstances.Single();

			Assert.AreEqual(1.0, surfaceImpl.LastDraw.Alpha);
			Assert.AreEqual(Color.White, surfaceImpl.LastDraw.Color);
			Assert.AreEqual(new PointF(44, 33), draw.DestLocation);
			Assert.AreEqual(new Rectangle(0, 0, 0, 0), draw.SourceRect);
			Assert.AreEqual(0, surfaceImpl.LastDraw.RotationAngle);
		}

		[TestMethod]
		public void DrawDestRect()
		{
			surface.Draw(new Rectangle(2, 3, 55, 66));

			var draw = surfaceImpl.LastDraw.DrawInstances.Single();

			Assert.AreEqual(1.0, surfaceImpl.LastDraw.Alpha);
			Assert.AreEqual(Color.White, surfaceImpl.LastDraw.Color);
			Assert.AreEqual(new PointF(2, 3), draw.DestLocation);
			Assert.AreEqual(new Rectangle(0, 0, 34, 30), draw.SourceRect);
			Assert.AreEqual(0, surfaceImpl.LastDraw.RotationAngle);
		}

		[TestMethod]
		public void DrawRectFs()
		{
			var srcRect = new RectangleF(2, 3, 5, 6);
			var destRect = new RectangleF(8, 9, 10, 12);

			surface.DrawRects(new[] { srcRect }, new[] { destRect });

			var draw = surfaceImpl.LastDraw.DrawInstances.Single();

			Assert.AreEqual(1.0, surfaceImpl.LastDraw.Alpha);
			Assert.AreEqual(Color.White, surfaceImpl.LastDraw.Color);
			Assert.AreEqual(new PointF(8, 9), draw.DestLocation);
			Assert.AreEqual(new Rectangle(2, 3, 5, 6), draw.SourceRect);
			Assert.AreEqual(0, surfaceImpl.LastDraw.RotationAngle);
			Assert.AreEqual(2, surfaceImpl.LastDraw.ScaleWidth);
			Assert.AreEqual(2, surfaceImpl.LastDraw.ScaleHeight);
		}

		[Obsolete]
		[TestMethod]
		public void DrawWithRotationCenter()
		{
			var srcRect = new Rectangle(2, 3, 5, 6);
			var destPt = new PointF(55, 66);
			var rotationPt = new PointF(15, 13);

			surface.RotationAngleDegrees = 60;
			surface.Draw(srcRect, destPt, rotationPt);

			var draw = surfaceImpl.LastDraw.DrawInstances.Single();

			Assert.AreEqual(new PointF(15, 13), surfaceImpl.LastDraw.RotationCenterLocation);
			Assert.AreEqual(Math.PI / 3.0, surfaceImpl.LastDraw.RotationAngle, 0.00001);
			Assert.AreEqual(1.0, surfaceImpl.LastDraw.Alpha);
			Assert.AreEqual(Color.White, surfaceImpl.LastDraw.Color);
			Assert.AreEqual(new PointF(55, 66), draw.DestLocation);
			Assert.AreEqual(new Rectangle(2, 3, 5, 6), draw.SourceRect);
			Assert.AreEqual(1, surfaceImpl.LastDraw.ScaleWidth);
			Assert.AreEqual(1, surfaceImpl.LastDraw.ScaleHeight);
		}

		[TestMethod]
		public void SurfaceConstruction()
		{
			var fileProvider = new FakeReadFileProvider();
			fileProvider.Add("test.png", "");

			surface = new Surface("test.png", fileProvider);

			Assert.AreEqual(1, fileProvider.ReadCount("test.png"));
		}

		[TestMethod]
		public void SurfaceRequiresFilename()
		{
			AssertX.Throws<ArgumentException>(() => new Surface((string)null));
		}
	}
}
