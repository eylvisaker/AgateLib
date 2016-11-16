﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.IO;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.UserInterface.DataModel
{
	[TestClass]
	public class ConfigReadingTests
	{
		string filename = "test.yaml";
		string yaml = @"
fonts:
  MedievalSharp:
  - name: MedievalSharp18
    image: Fonts/MedievalSharp18.png
    metrics:
      32:
        x: 0
        y: 2
        width: 8
        height: 30
        right-overhang: 1";

		
		[TestInitialize]
		public void Initialize()
		{
			var fileProvider = new Mock<IReadFileProvider>();
			fileProvider
				.Setup(x => x.OpenReadAsync(filename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(yaml))));

			Assets.UserInterfaceAssets = fileProvider.Object;
		}

		[TestMethod]
		public void ReadFontModel()
		{
			var configModel = UserInterfaceDataLoader.Config(filename);

			Assert.AreEqual(1, configModel.Fonts.Count);
			Assert.AreEqual("MedievalSharp", configModel.Fonts.Keys.First());

			var font = configModel.Fonts["MedievalSharp"].First();

			Assert.AreEqual("Fonts/MedievalSharp18.png", font.Image);
			Assert.AreEqual(32, font.Metrics.Keys.First());
			Assert.AreEqual(1, font.Metrics.Values.First().RightOverhang);
		}

		[TestMethod]
		public void WriteFontModel()
		{
			var configModel = new UserInterfaceConfig();
			var fontModel = new FontModel
			{
				Name = "MedievalSharp18",
				Image = "Fonts/MedievalSharp18.png",
			};

			fontModel.Metrics.Add(32, new GlyphMetrics { Y = 2, Width = 8, Height = 30, RightOverhang = 1 });

			configModel.Fonts.Add("MedievalSharp", new List<FontModel> { fontModel });

			Serializer ser = new Serializer();

			Console.WriteLine(ser.Serialize(configModel));
		}
	}
}
