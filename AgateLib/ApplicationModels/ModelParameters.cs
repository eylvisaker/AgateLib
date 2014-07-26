using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public class ModelParameters
	{
		string[] args;

		public ModelParameters()
		{
			CreateFullScreenWindow = true;
		}
		public ModelParameters(string[] args) : this()
		{
			Arguments = args;
		}

		public string ApplicationName { get; set; }

		public bool AutoCreateDisplayWindow { get; set; }
		public Size DisplayWindowSize { get; set; }
		public bool CreateFullScreenWindow { get; set; }

		public string[] Arguments
		{
			get { return args; }
			set
			{
				args = value;
				ParseArguments();
			}
		}

		private void ParseArguments()
		{
		}

	}
}
