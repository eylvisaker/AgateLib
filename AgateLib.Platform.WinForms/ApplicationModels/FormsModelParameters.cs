using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public class FormsModelParameters : ModelParameters
	{
		public FormsModelParameters()
		{
			AssetPath = ".";
		}

		public string AssetPath { get; set; }
	}
}
