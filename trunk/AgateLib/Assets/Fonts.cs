using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Assets
{
	/// <summary>
	/// Collection of default fonts.
	/// </summary>
	public static class Fonts
	{
		public static void Initialize()
		{
			AgateSans = Font.Create("AgateSans", 8, 64, FontStyles.Bold);
			AgateSerif = Font.Create("AgateSerif", 8, 64, FontStyles.Bold);
			AgateMono = Font.Create("AgateMono", 8, 64, FontStyles.Bold);

			Display.DisposeDisplay += Display_DisposeDisplay;
		}

		static void Display_DisposeDisplay()
		{
			AgateSans.Dispose();
			AgateSerif.Dispose();
			AgateMono.Dispose();

			AgateSans = null;
			AgateSerif = null;
			AgateMono = null;

			Display.DisposeDisplay -= Display_DisposeDisplay;
		}

		/// <summary>
		/// Default sans serif font.
		/// </summary>
		public static Font AgateSans { get; private set; }
		/// <summary>
		/// Default serif font.
		/// </summary>
		public static Font AgateSerif { get; private set; }
		/// <summary>
		/// Default monospace font.
		/// </summary>
		public static Font AgateMono { get; private set; }
	}
}
