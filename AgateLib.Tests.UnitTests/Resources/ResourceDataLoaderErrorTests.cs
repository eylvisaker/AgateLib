using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.Test;
using AgateLib.Quality;
using AgateLib.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class ResourceDataLoaderErrorTests
	{
		[TestMethod]
		public void FileNotFound()
		{
			FakeReadFileProvider fileProvider = new FakeReadFileProvider();
			fileProvider.Add("resources.yaml", @"
facet-sources:
-   file/is/missing/yaml");

			var loader = new ResourceDataLoader(fileProvider);

			AssertX.Throws<FileNotFoundException>(() => loader.Load("resources.yaml"));
		}
	}
}
