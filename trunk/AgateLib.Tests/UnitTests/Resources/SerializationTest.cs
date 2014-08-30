using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.Resources.DC;
using AgateLib.Sprites;
using AgateLib.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Resources
{
	[TestClass]
	public class SerializationTest
	{
		[TestMethod]
		public void SerializeFontResource()
		{
			AgateResourceCollection rc = new AgateResourceCollection();
			AgateLib.Core.Initialize(new FakeAgateFactory());
			AgateLib.Core.InitAssetLocations(new AssetLocations());

			rc.Resources.Add("mainfont",
				new FontResource("times new roman",
					new FontSurfaceResource { ImageFilename = "SomeFile.png", FontMetrics = CreateFontMetrics(8), FontSettings = new FontSettings(8, FontStyles.None) },
					new FontSurfaceResource { ImageFilename = "OtherFile.png", FontMetrics = CreateFontMetrics(10), FontSettings = new FontSettings(10, FontStyles.Bold) }));

			MemoryStream str = new MemoryStream();
			rc.SaveTo(str);
			str.Seek(0, SeekOrigin.Begin);

			AgateResourceCollection rs = new AgateResourceCollection(str);

			var fr = (FontResource)rs.Resources["mainfont"];
			var fs = fr.FontSurfaces;

			Assert.AreEqual("times new roman", fr.FontName);
			Assert.AreEqual("SomeFile.png", fs[0].ImageFilename);
			Assert.AreEqual(8, fs[0].FontSettings.Size);
			Assert.AreEqual(FontStyles.None, fs[0].FontSettings.Style);

			Assert.AreEqual("OtherFile.png", fs[1].ImageFilename);
			Assert.AreEqual(10, fs[1].FontSettings.Size);
			Assert.AreEqual(FontStyles.Bold, fs[1].FontSettings.Style);

			Assert.AreEqual(8, fs[0].FontMetrics['C'].Height);
			Assert.AreEqual(10, fs[1].FontMetrics['C'].Height);
		}

		private FontMetrics CreateFontMetrics(int size)
		{
			FontMetrics metrics = new FontMetrics();

			for (char i = 'A'; i < 'z'; i++)
			{
				metrics.Add(i, new GlyphMetrics
				{
					Size = new Geometry.Size(size, size),
					SourceRect = new Rectangle(i * size, 0, size, size),
					LeftOverhang = i % 2,
					RightOverhang = i % 4,
				});
			}

			return metrics;
		}

		[TestMethod]
		public void SerializeSpriteResources()
		{
			AgateResourceCollection rc = new AgateResourceCollection();

			SpriteFrameResource sfr = new SpriteFrameResource
			{
				Anchor = new Point(4, 4),
				ImageFilename = "sprite.png",
				SourceRect = new Rectangle(10, 20, 30, 40),
			};

			SpriteResource sr = new SpriteResource();

			sr.Frames.Add(sfr);
			sr.AnimType = SpriteAnimType.Once;
			sr.TimePerFrame = 100;

			rc.Resources.Add("Sprite", sr);

			MemoryStream str = new MemoryStream();
			rc.SaveTo(str);
			str.Seek(0, SeekOrigin.Begin);

			File.WriteAllBytes(@"temp.txt", str.GetBuffer());

			AgateResourceCollection rs = new AgateResourceCollection(str);

			var sd = (SpriteResource)rs.Resources["Sprite"];

			Assert.AreEqual(100, sd.TimePerFrame);
			Assert.AreEqual(sr.TimePerFrame, sd.TimePerFrame);
			Assert.AreEqual(sr.AnimType, sd.AnimType);
			Assert.AreEqual(SpriteAnimType.Once, sd.AnimType);
			Assert.AreEqual(new Point(4, 4), sd.Frames[0].Anchor);
			Assert.AreEqual("sprite.png", sd.Frames[0].ImageFilename);
			Assert.AreEqual(new Rectangle(10, 20, 30, 40), sd.Frames[0].SourceRect);
		}
	}
}
