using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests
{
	public interface IAgateTest
	{
		string Name { get; }

		string Category { get; }

		void Run(string[] args);
	}
}
