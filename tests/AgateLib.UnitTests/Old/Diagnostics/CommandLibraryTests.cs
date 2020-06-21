using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.ConsoleSupport;
using Xunit;

namespace AgateLib.UnitTests.Diagnostics
{
	[TestClass]
	public class CommandLibraryTests : AgateUnitTest
	{
		class Vocabulary : IVocabulary
		{
			public string Namespace => "";

			public int Callme_Count { get; private set; }
			public string Callme_parameter1 { get; private set; }
			public string Callme_parameter2 { get; private set; }

			[ConsoleCommand("Test method.")]
			public void Callme(string parameter1, string parameter2)
			{
				Callme_Count++;
				Callme_parameter1 = parameter1;
				Callme_parameter2 = parameter2;

			}
		}

		Vocabulary library = new Vocabulary();
		AgateConsoleCore console = new AgateConsoleCore();

		[TestInitialize]
		public void Initialize()
		{
			AgateConsole.Initialize(console);
		}

		[TestMethod]
		public void CallLibraryCommand()
		{
			AgateConsole.CommandLibraries.Add(new LibraryVocabulary(library));

			console.ProcessKeys("callme p1 p2\n");

			Assert.AreEqual(1, library.Callme_Count);
			Assert.AreEqual("p1", library.Callme_parameter1);
			Assert.AreEqual("p2", library.Callme_parameter2);
		}
	}
}
