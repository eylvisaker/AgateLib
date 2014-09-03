using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	public class SceneModelParameters : ModelParameters
	{
		/// <summary>
		/// Constructs a SceneModelParameters object with default parameters. 
		/// </summary>
		public SceneModelParameters()
		{ }
		/// <summary>
		/// Constructs a SceneModelParameters object with default parameters. 
		/// Also stores the command line arguments.
		/// </summary>
		/// <param name="args"></param>
		public SceneModelParameters(string[] args)
			: base(args)
		{ }
	}
}
