using System;
using System.IO;
using System.Reflection;
using AgateLib.Platform.IntegrationTest;

namespace AgateLib.UnitTests
{
	public class AgateIntegrationTest : IDisposable
	{
		AgateIntegrationTestPlatform platform;

		string appDirPath;

		public AgateIntegrationTest()
		{
			appDirPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AgateIntegrationTest)).Location);

			platform = new AgateIntegrationTestPlatform();
		}

		public void Dispose()
		{
			Dispose(true);

			platform.Dispose();
		}

		protected virtual void Dispose(bool disposing)
		{
			
		}

		//[TestMethod, Ignore]
		//public void ReadTextFile()
		//{
		//    string filePath = Path.Combine(appDirPath, "hello.txt");
		//    string text = "This is a test.";

		//    File.WriteAllText(filePath, text);

		//    using (var fin = new StreamReader(Assets.OpenRead("hello.txt")))
		//    {
		//        var result = fin.ReadToEnd();

		//        Assert.AreEqual(text, result);
		//    }
		//}
	}
}
