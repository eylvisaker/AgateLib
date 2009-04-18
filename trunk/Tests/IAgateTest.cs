using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
	public interface IAgateTest
	{
		string Name { get; }
		string Category { get; }
		void Main(string[] args);
	}
}
