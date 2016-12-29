using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Quality;

namespace AgateLib.Configuration
{
	public class AgateConfig
	{
		IReadOnlyList<DisplayWindow> displayWindows = new List<DisplayWindow>();

		/// <summary>
		/// Gets the list of display windows that were created during initialization.
		/// </summary>
		public IReadOnlyList<DisplayWindow> DisplayWindows
		{
			get { return displayWindows; }
			set
			{
				Condition.RequireArgumentNotNull(value, nameof(DisplayWindows));
				displayWindows = value;
			}
		}
	}
}
