using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing
{
	/// <summary>
	/// Test based on a custom application model
	/// </summary>
	public interface IDiscreteAgateTest : IAgateTest
	{
		void Main(string[] args);
	}
}
