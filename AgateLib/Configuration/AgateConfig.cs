using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;

namespace AgateLib.Configuration
{
	public class AgateConfig
	{
		/// <summary>
		/// Gets the list of display windows that were created during initialization.
		/// </summary>
		public IReadOnlyList<DisplayWindow> DisplayWindows { get; internal set; }
	}
}
