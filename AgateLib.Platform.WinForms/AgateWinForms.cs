using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.DisplayImplementation;
using AgateLib.Platform.WinForms.Factories;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Quality;

namespace AgateLib.Platform.WinForms
{
	public class AgateWinForms : AgateSetupCore
	{
		public static AgateWinForms Initialize(string[] args)
		{
			return new AgateWinForms(args);
		}

		public AgateWinForms(string[] commandLineArguments = null)
		{
			ParseCommandLineArgs(commandLineArguments);

			AgateApp.Initialize(new FormsFactory());
		}
	}
}
