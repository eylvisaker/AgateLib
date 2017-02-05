using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.Settings
{
	[TestClass]
	public class SettingsTests
	{
		class SettingsData
		{
			public int IntValue { get; set; }
			public double DoubleValue { get; set; }
		}

		[TestMethod]
		public void ReadSettings()
		{
			using (var platform = new AgateUnitTestPlatform()
				.Initialize())
			{
				platform.UserAppDataFileProvider.Add("Settings/test.settings", @"
IntValue: 4
DoubleValue: 8.9");

				var settings = AgateApp.Settings.Get<SettingsData>("test");

				Assert.AreEqual(4, settings.IntValue);
				Assert.AreEqual(8.9, settings.DoubleValue, 0.0001);

			}
		}

		[TestMethod]
		public void SaveSettings()
		{
			using (var platform = new AgateUnitTestPlatform()
				.Initialize())
			{
				var settings = AgateApp.Settings.GetOrCreate("test",
					() => new SettingsData());

				settings.IntValue = 37;
				settings.DoubleValue = 0.25;

				AgateApp.Settings.Save();

				var text = platform.UserAppDataFileProvider.ReadAllText("Settings/test.settings");

				var deser = new DeserializerBuilder().Build();

				var actual = deser.Deserialize<SettingsData>(new StringReader(text));

				Assert.AreEqual(37, actual.IntValue);
				Assert.AreEqual(0.25, actual.DoubleValue);
			}
		}
	}
}
