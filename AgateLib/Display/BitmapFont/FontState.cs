//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace AgateLib.Display.BitmapFont
{
	/// <summary>
	/// Class which represents the state information used to draw texdt on the screen.
	/// </summary>
	public class FontState
	{
		private OriginAlignment alignment = OriginAlignment.TopLeft;
		private Color color = Color.White;
		private double scaleWidth = 1.0;
		private double scaleHeight = 1.0;
		private Vector2 location;
		private string text = "";
		private IFontStateCache cache;
		private int size = 10;
		private FontStyles style;
		
		/// <summary>
		/// Gets or sets the text that is displayed when drawn.
		/// </summary>
		public string Text
		{
			get => text;
			set
			{
				text = value;

				if (Cache != null)
					Cache.OnTextChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the location where text is drawn.
		/// </summary>
		public Vector2 Location
		{
			get => location;
			set
			{
				location = value;

				if (Cache != null)
					Cache.OnLocationChanged(this);
			}
		}
		/// <summary>
		/// Sets how to interpret the point given to DrawText methods.
		/// </summary>
		public OriginAlignment TextAlignment
		{
			get => alignment;
			set
			{
				alignment = value;

				Cache?.OnDisplayAlignmentChanged(this);
			}
		}
		/// <summary>
		/// Sets the color of the text to be drawn.
		/// </summary>
		public Color Color
		{
			get => color;
			set
			{
				color = value;

				if (Cache != null)
					Cache.OnColorChanged(this);
			}
		}
		/// <summary>
		/// Sets the alpha value of the text to be drawn.
		/// </summary>
		public double Alpha
		{
			get => color.A / 255.0;
			set
			{
				if (value < 0) value = 0;
				if (value > 1.0) value = 1.0;

				color = new Color(color, (int)(value * 255));

				Cache?.OnColorChanged(this);
			}
		}

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		public int Size
		{
			get => size;
			set
			{
				size = value;

				Cache?.OnSizeChanged(this);
			}
		}

		public FontStyles Style
		{
			get => style;
			set
			{
				style = value;

				Cache?.OnStyleChanged(this);
			}
		}
		/// <summary>
		/// Gets or sets the amount the width is scaled when the text is drawn.
		/// 1.0 is no scaling.
		/// </summary>
		internal double ScaleWidth
		{
			get => scaleWidth;
			set
			{
				scaleWidth = value;

				if (Cache != null)
					Cache.OnScaleChanged(this);
			}
		}
		/// <summary>
		/// Gets or sets the amount the height is scaled when the text is drawn.
		/// 1.0 is no scaling.
		/// </summary>
		internal double ScaleHeight
		{
			get => scaleHeight;
			set
			{
				scaleHeight = value;

				if (Cache != null)
					Cache.OnScaleChanged(this);
			}
		}
		/// <summary>
		/// This value is used by the implementation to optimize rendering this state object.
		/// Do not set this value unless you know what you are doing, or writing an implementation
		/// of FontSurfaceImpl.
		/// </summary>
		public IFontStateCache Cache
		{
			get => cache;
			set => cache = value;
		}
		
		public FontSettings Settings => new FontSettings(Size, style);

		#region --- ICloneable Members ---

		/// <summary>
		/// Returns a deep copy of the FontState object.
		/// </summary>
		/// <returns></returns>
		public FontState Clone()
		{
			FontState result = new FontState();

			result.alignment = alignment;
			result.color = color;
			result.scaleWidth = scaleWidth;
			result.scaleHeight = scaleHeight;
			result.location = location;
			result.text = text;
			result.Size = size;
			result.Style = Style;

			if (cache != null)
			{
				result.cache = cache.Clone();
			}

			return result;
		}

		#endregion
	}

}