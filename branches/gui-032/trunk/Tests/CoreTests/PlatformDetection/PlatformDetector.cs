using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.CoreTests.PlatformDetection
{
	class PlatformDetector: IAgateTest
	{
		#region IAgateTest Members

		string IAgateTest.Name
		{
			get { return "Platform Detection"; }
		}

		string IAgateTest.Category
		{
			get { return "Core"; }
		}

		void IAgateTest.Main(string[] args)
		{
			new PlatformDetection().ShowDialog();
		}

		#endregion
	}
}
