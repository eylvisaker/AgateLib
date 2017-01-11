using AgateLib.ApplicationModels;
using AgateLib.IO;
using AgateLib.Platform.IntegrationTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Platform.IntegrationTest
{
	[TestClass]
	public class AgateIntegrationTest : IDisposable
	{
		IntegrationTestPlatform platform;

		string appDirPath;

		public AgateIntegrationTest()
		{
			appDirPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AgateIntegrationTest)).Location);

			var parameters = new SerialModelParameters();

			platform = new IntegrationTestPlatform();
			platform.InitializeAgateLib();
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
