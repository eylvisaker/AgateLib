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
		static DefaultResources mResources;

		internal static void Initialize(DefaultResources res)
		{
			mResources = res;

			Display.DisposeDisplay += Display_DisposeDisplay;
		}

		static void Display_DisposeDisplay()
		{
			mResources.Dispose();
			mResources = null;

			Display.DisposeDisplay -= Display_DisposeDisplay;
		}

		/// <summary>
		/// Default sans serif font.
		/// </summary>
		public static Font AgateSans { get { return mResources.AgateSans; } }
		/// <summary>
		/// Default serif font.
		/// </summary>
		public static Font AgateSerif { get { return mResources.AgateSerif; } }
		/// <summary>
		/// Default monospace font.
		/// </summary>
		public static Font AgateMono { get { return mResources.AgateMono; } }
	}
}
