using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AgateLib.Tests.FontTests
{
	public class PaletteC64 : IEnumerable<Color>
	{
		/// <summary>
		/// C64 color palette
		/// </summary>
		private readonly List<Color> colors = new List<Color>
		{
			new Color(0,0,0),
			new Color(255,255,255),
			new Color(136,0,0),
			new Color(170,255,238),
			new Color(204,68,204),
			new Color(0, 204, 85),
			new Color(0,0,170),
			new Color(238,238,119),
			new Color(221,136,85),
			new Color(102, 68,0),
			new Color(255,119,119),
			new Color(51,51,51),
			new Color(119,119,119),
			new Color(170,255,102),
			new Color(0, 136, 255),
			new Color(187,187,187),
		};

		public int Count => colors.Count;

		public Color this[int index] => colors[index];

		public IEnumerator<Color> GetEnumerator()
		{
			return colors.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
