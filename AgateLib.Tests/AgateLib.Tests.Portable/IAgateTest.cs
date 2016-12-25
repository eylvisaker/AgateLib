using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Tests
{
	/// <summary>
	/// Interface for a test class. Do not implement this interface directly, instead
	/// implement ISceneModelTest, ISerialModelTest or IDiscreteAgateTest
	/// </summary>
	public interface IAgateTest
	{
		string Name { get; }
		string Category { get; }
	}
}
