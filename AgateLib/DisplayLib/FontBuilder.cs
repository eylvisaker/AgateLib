using System;
using AgateLib.Quality;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Constructs a font from a set of font surfaces.
	/// </summary>
	public class FontBuilder
	{
		private Font font;

		public FontBuilder(string name)
		{
			font = new Font(name);
		}

		/// <summary>
		/// Adds a font surface to the font.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="surface"></param>
		/// <returns></returns>
		public FontBuilder AddFontSurface(FontSettings settings, FontSurface surface)
		{
			Condition.Requires<InvalidOperationException>(font != null,
				"FontBuilder objects cannot be reused.");

			font.AddFontSurface(settings, surface);

			return this;
		}

		/// <summary>
		/// Builds the resulting Font object.
		/// </summary>
		/// <returns></returns>
		public Font Build()
		{
			var result = font;

			font = null;

			return result;
		}
	}
}