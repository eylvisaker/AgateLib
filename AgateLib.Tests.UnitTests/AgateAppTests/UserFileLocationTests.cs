using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.AgateAppTests
{
	[TestClass]
	public class UserFileLocationTests
	{
		[TestMethod]
		public void UserFileIsNotNull()
		{
			using (new AgateUnitTestPlatform().Initialize())
			{
				Assert.IsNotNull(AgateApp.UserFiles);
			}
		}
	}
}
