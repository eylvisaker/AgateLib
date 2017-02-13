using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which provides information about a physical screen attached to 
	/// the user's system.
	/// </summary>
	public class ScreenInfo
	{
		/// <summary>
		/// Gets the name of the screen.
		/// </summary>
		public string DeviceName { get; set; }

		/// <summary>
		/// Gets the desktop boundaries of the screen.
		/// </summary>
		public Rectangle Bounds { get; set; }

		/// <summary>
		/// Gets a value indicating whether the screen is the primary.
		/// </summary>
		public bool IsPrimary { get; set; }

		/// <summary>
		/// Gets the full screen display window assigned to this screen.
		/// </summary>
		[Obsolete("Use DisplayWindow.Screen instead.")]
		public DisplayWindow DisplayWindow { get; set; }

		/// <summary>
		/// An IntPtr that can be used by the rendering system to track 
		/// this monitor reference.
		/// </summary>
		protected internal IntPtr SystemIndex { get; set; }
	}
}
