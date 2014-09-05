using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.Platform.WinForms.IO;

namespace AgateLib.IO
{
	[TestClass]
	public class FileSystemProviderTests
	{
		[TestMethod]
		public void FspRelativePath()
		{
			FileSystemProvider fsp = new FileSystemProvider(@"c:\test\path");

			Assert.AreEqual("to/somewhere.txt", 
				fsp.MakeRelativePath(@"c:\test\path\to\somewhere.txt"));

			Assert.AreEqual("../alternate",
				fsp.MakeRelativePath(@"c:\test\alternate"));
		}
	}
}
