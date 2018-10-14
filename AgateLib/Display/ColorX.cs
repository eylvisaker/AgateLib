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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AgateLib.Display
{
	public static class ColorX
	{
		/// <summary>
		/// Tries to parse the color. First calls TryParseNamedColor to see if the value matches a known name,
		/// then TryParseFromArgb.
		/// </summary>
		/// <param name="color">The string to parse. It can either be a name like "red", a RRGGBB value like "f0a066" or an 
		/// ARGB value like "fff0a066".</param>
		/// <param name="result">The resulting color.</param>
		/// <returns></returns>
		public static bool TryParse(string color, out Color result)
		{
			if (TryParseNamedColor(color, out result))
			{
				return true;
			}

			return TryParseFromArgb(color, out result);
		}

		/// <summary>
		/// Checks if the given name matches a color and returns the value in the out parameter
		/// if it does.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static bool TryParseNamedColor(string name, out Color result)
		{
			var colorType = typeof(Color).GetTypeInfo();

			var prop = colorType.DeclaredProperties
				.FirstOrDefault(x =>
					x.PropertyType == typeof(Color) &&
					x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			
			var getMethod = prop?.GetMethod;

			if ((!getMethod?.IsStatic) ?? true)
			{
				result = default(Color);
				return false;
			}

			if (!getMethod.IsPublic)
			{
				result = default(Color);
				return false;
			}

			result = (Color)getMethod.Invoke(null, null);
			return true;
		}

        /// <summary>
        /// Tries to convert a string to a Microsoft.Xna.Framework.Color structure.
        /// </summary>
        /// <param name="str">The string to convert.  It must be in one of the following formats
        /// RRGGBB, AARRGGBB, 0xRRGGBB, 0xAARRGGBB where AA, RR, GG, BB are each a hexidecimal
        /// number (such as "ff" or "8B").</param>
        /// <param name="result">The resulting color.</param>
        /// <returns></returns>
        public static bool TryParseFromArgb(string str, out Color result)
		{
			if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
				str = str.Substring(2);

			if (str.Length == 6)
			{
				byte r, g, b;

				bool valid = true;

				valid &= TryParseByteValueFromHex(str.Substring(0, 2), out r);
				valid &= TryParseByteValueFromHex(str.Substring(2, 2), out g);
				valid &= TryParseByteValueFromHex(str.Substring(4, 2), out b);

				if (!valid)
				{
					result = default(Color);
					return false;
				}

				result = new Color(r, g, b);

				return true;
			}

			if (str.Length == 8)
			{
				byte a, r, g, b;

				bool valid = true;

				valid &= TryParseByteValueFromHex(str.Substring(0, 2), out a);
				valid &= TryParseByteValueFromHex(str.Substring(2, 2), out r);
				valid &= TryParseByteValueFromHex(str.Substring(4, 2), out g);
				valid &= TryParseByteValueFromHex(str.Substring(6, 2), out b);

				if (!valid)
				{
					result = default(Color);
					return false;
				}

				result = new Color(r, g, b);

				return true;
			}

			result = new Color();

			return false;
		}

        /// <summary>
        /// Convert a color to ARGB hex representation.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToArgb(this Color color)
        {
            return $"{color.A:x2}{color.R:x2}{color.G:x2}{color.B:x2}";
        }

        /// <summary>
        /// Converts a string to an Microsoft.Xna.Framework.Color structure.
        /// </summary>
        /// <param name="str">The string to convert.  It must be in one of the following formats
        /// RRGGBB, AARRGGBB, 0xRRGGBB, 0xAARRGGBB where AA, RR, GG, BB are each a hexidecimal
        /// number (such as "ff" or "8B").</param>
        /// <returns></returns>
        public static Color FromArgb(string str)
		{
			if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
				str = str.Substring(2);

			if (str.Length == 6)
			{
				byte r = ByteValueFromHex(str.Substring(0, 2));
				byte g = ByteValueFromHex(str.Substring(2, 2));
				byte b = ByteValueFromHex(str.Substring(4, 2));

				return new Color(r, g, b);
			}
			else if (str.Length == 8)
			{
				byte a = ByteValueFromHex(str.Substring(0, 2));
				byte r = ByteValueFromHex(str.Substring(2, 2));
				byte g = ByteValueFromHex(str.Substring(4, 2));
				byte b = ByteValueFromHex(str.Substring(6, 2));

				return new Color(r, g, b, a);
			}
			else
				throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture,
					"Argument \"{0}\" is not a valid Color string.", str));
		}

        /// <summary>
        /// Converts a string to an Microsoft.Xna.Framework.Color structure.
        /// </summary>
        /// <param name="str">The string to convert.  It must be in one of the following formats
        /// BBGGRR, AABBGGRR, 0xBBGGRR, 0xAABBGGRR where AA, RR, GG, BB are each a hexidecimal
        /// number (such as "ff" or "8B").</param>
        /// <returns></returns>
        public static Color FromAbgr(string str)
        {
            if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                str = str.Substring(2);

            if (str.Length == 6)
            {
                byte b = ByteValueFromHex(str.Substring(0, 2));
                byte g = ByteValueFromHex(str.Substring(2, 2));
                byte r = ByteValueFromHex(str.Substring(4, 2));

                return new Color(r, g, b);
            }
            else if (str.Length == 8)
            {
                byte a = ByteValueFromHex(str.Substring(0, 2));
                byte b = ByteValueFromHex(str.Substring(2, 2));
                byte g = ByteValueFromHex(str.Substring(4, 2));
                byte r = ByteValueFromHex(str.Substring(6, 2));

                return new Color(r, g, b, a);
            }
            else
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                    "Argument \"{0}\" is not a valid Color string.", str));
        }

        /// <summary>
        /// Converts a string like "FF" to a byte value.  Throws an exception if the
        /// string does not convert to a value which fits into a byte.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static bool TryParseByteValueFromHex(string hex, out byte value)
		{
			value = 0;

			if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int ivalue))
			{
				if (ivalue > 255 || ivalue < 0)
					return false;

				value = (byte)ivalue;

				return true;
			}

			return false;
		}

		/// <summary>
		/// Converts a string like "FF" to a byte value.  Throws an exception if the
		/// string does not convert to a value which fits into a byte.
		/// </summary>
		/// <param name="hex"></param>
		/// <returns></returns>
		private static byte ByteValueFromHex(string hex)
		{
			int value;

			if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out value))
			{
				if (value > 255 || value < 0)
					throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture,
						"Invalid result.  Input Hex number: {0}, Result: {1}", hex, value));

				return (byte)value;
			}
			else
				throw new ArgumentException("Not a hex number.");
		}


		// See algorithm at http://en.wikipedia.org/wiki/YUV#Conversion_to.2Ffrom_RGB
		const double W_R = 0.299;
		const double W_B = 0.114;
		const double W_G = 0.587;
		const double Umax = 0.436;
		const double Vmax = 0.615;

		/// <summary>
		/// Converts a color to YUV values.
		/// </summary>
		/// <param name="y"></param>
		/// <param name="u"></param>
		/// <param name="v"></param>
		public static void ToYuv(Color clr, out double y, out double u, out double v)
		{
			y = (W_R * clr.R + W_G * clr.G + W_B * clr.B) / 255.0;
			u = Umax * (clr.B / 255.0 - y) / (1 - W_B);
			v = Vmax * (clr.R / 255.0 - y) / (1 - W_R);
		}

		/// <summary>
		/// Creates a color from YUV values.
		/// </summary>
		/// <param name="y"></param>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public static Color FromYuv(double y, double u, double v)
		{
			return new Color(
				(int)(255 * (y + v * (1 - W_R) / Vmax)),
				(int)(255 * (y - u * W_B * (1 - W_B) / (Umax * W_G) - v * W_R * (1 - W_R) / (Vmax * W_G))),
				(int)(255 * (y + u * (1 - W_B) / Umax)));
		}

		/// <summary>
		/// Returns a Color object calculated from hue, saturation and value.
		/// See algorithm at http://en.wikipedia.org/wiki/HSL_and_HSV#From_HSV
		/// </summary>
		/// <param name="hue">The hue angle in degrees.</param>
		/// <param name="saturation">A value from 0 to 1 representing saturation.</param>
		/// <param name="value">A value from 0 to 1 representing the value.</param>
		/// <returns></returns>
		public static Color FromHsv(double hue, double saturation, double value)
		{
			while (hue < 0)
				hue += 360;
			if (hue >= 360)
				hue = hue % 360;

			double hp = hue / 60;
			double chroma = value * saturation;
			double x = chroma * (1 - Math.Abs(hp % 2 - 1));

			double r1 = 0, b1 = 0, g1 = 0;

			switch ((int)hp)
			{
				case 0: r1 = chroma; g1 = x; break;
				case 1: r1 = x; g1 = chroma; break;
				case 2: g1 = chroma; b1 = x; break;
				case 3: g1 = x; b1 = chroma; break;
				case 4: r1 = x; b1 = chroma; break;
				case 5: r1 = chroma; b1 = x; break;
			}

			double m = value - chroma;

			return new Color((int)(255 * (r1 + m)), (int)(255 * (g1 + m)), (int)(255 * (b1 + m)));
		}

		/// <summary>
		/// Returns a number from 0.0 to 1.0 indicating the intensity of the color, normalized
		/// in a way which approximately represents the human eye's response to color.
		/// </summary>
		public static double IntensityOf(Color color)
		{
			return (0.30 * color.R + 0.59 * color.G + 0.11 * color.B) / 255.0;
		}
	}
}
