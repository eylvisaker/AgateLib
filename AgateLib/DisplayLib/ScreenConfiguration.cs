using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Lists the physical monitors attached to the system.
	/// </summary>
	public interface IScreenConfiguration
	{
		/// <summary>
		/// Lists all the screens on the system.
		/// </summary>
		IReadOnlyList<ScreenInfo> AllScreens { get; }

		/// <summary>
		/// Returns the primary screen, as designated by the user's desktop preferences.
		/// </summary>
		ScreenInfo PrimaryScreen { get; }
	}
}
