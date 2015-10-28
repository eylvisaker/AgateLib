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
    public class AgateIntegrationTest
    {
        string appDirPath;

        [TestInitialize]
        public void Initialize()
        {
            appDirPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AgateIntegrationTest)).Location);
            var parameters = new SerialModelParameters();

            IntegrationTestPlatform.Initialize(parameters, appDirPath);
        }

        [TestMethod]
        public void ReadTextFile()
         {
            string filePath = Path.Combine(appDirPath, "hello.txt");
            string text = "This is a test.";

            File.WriteAllText(filePath, text);

            using (var fin = new StreamReader(Assets.OpenRead(filePath)))
            {
                var result = fin.ReadToEnd();

                Assert.AreEqual(text, result);
            }
        }
    }
}
