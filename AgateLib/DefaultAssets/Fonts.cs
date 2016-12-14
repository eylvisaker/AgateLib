using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DefaultAssets
{
	/// <summary>
	/// Collection of default fonts.
	/// </summary>
	public static class Fonts
	{
		internal static void Initialize(DefaultResources res)
		{
			Core.State.Display.DefaultResources = res;

			Display.DisposeDisplay += Display_DisposeDisplay;
		}

		static void Display_DisposeDisplay()
		{
			Core.State.Display.DefaultResources.Dispose();
			Core.State.Display.DefaultResources = null;

			Display.DisposeDisplay -= Display_DisposeDisplay;
		}

		/// <summary>
		/// Default sans serif font.
		/// </summary>
		public static IFont AgateSans
		{
			get { return Core.State.Display.DefaultResources.AgateSans; }
		}

		/// <summary>
		/// Default serif font.
		/// </summary>
		public static IFont AgateSerif
		{
			get { return Core.State.Display.DefaultResources.AgateSerif; }
		}

		/// <summary>
		/// Default monospace font.
		/// </summary>
		public static IFont AgateMono
		{
			get { return Core.State.Display.DefaultResources.AgateMono; }
		}
	}
}
