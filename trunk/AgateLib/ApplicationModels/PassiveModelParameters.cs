using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public class PassiveModelParameters : ModelParameters
	{
		public PassiveModelParameters()
		{
			AutoCreateDisplayWindow = false;
		}

		public PassiveModelParameters(string[] args) : this()
		{
			Arguments = args;
		}
	}
}
