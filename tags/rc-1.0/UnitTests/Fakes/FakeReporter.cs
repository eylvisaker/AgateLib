using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Drivers;

namespace AgateLib.UnitTests.Fakes
{
	class FakeReporter : AgateDriverReporter
	{
		public override IEnumerable<AgateDriverInfo> ReportDrivers()
		{
			yield return new AgateDriverInfo((DisplayTypeID)1000, typeof(FakeDisplayDriver), "Fake Display Driver", 1000);
		}
	}
}
