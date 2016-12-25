using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests
{
	/// <summary>
	/// Test based on a serial application model.
	/// </summary>
	public interface ISerialModelTest : IAgateTest
	{
		void ModifyModelParameters(SerialModelParameters parameters);
		void EntryPoint();
	}
}
