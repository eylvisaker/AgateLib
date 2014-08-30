using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public class SerialModelParameters : ModelParameters
	{
		public SerialModelParameters()
		{
		}

		public SerialModelParameters(string[] args)
			: this()
		{
			Arguments = args;
		}
	}
}
