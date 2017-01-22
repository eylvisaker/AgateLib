using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms
{
	public class AgateWinForms : AgateSetup
	{
		public static AgateWinForms Initialize(string[] args)
		{
			return new AgateWinForms(args);
		}

		public AgateWinForms(string[] commandLineArguments = null) : base(commandLineArguments)
		{
		}
	}
}
