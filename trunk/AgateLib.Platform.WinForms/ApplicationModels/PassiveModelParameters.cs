using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public class PassiveModelParameters : FormsModelParameters
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
