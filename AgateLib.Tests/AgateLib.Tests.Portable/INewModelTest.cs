using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;

namespace AgateLib.Tests
{
	public interface INewModelTest : IAgateTest
	{
		AgateConfig Configuration { get; set; }

		void ModifySetup(IAgateSetup setup);

		void Run();
	}
}
