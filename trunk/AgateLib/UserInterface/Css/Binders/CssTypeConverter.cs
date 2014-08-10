using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Binders
{
	public static class CssTypeConverter
	{
		public static object ChangeType(Type targetType, string value)
		{
			if (targetType == typeof(string))
				return value;
			if (targetType == typeof(Color))
				return ParseColor(value);
			if (targetType == typeof(CssDistance))
				return CssDistance.FromString(value);
			if (targetType.IsEnum)
				return Enum.Parse(targetType, value.Replace("-", "_"), true);

			throw new NotImplementedException();
		}

		public static bool TryParseColor(string value, out Color clr)
		{
			clr = Color.FromArgb(0, 0, 0, 0);

			if (Color.IsNamedColor(value))
			{
				clr = Color.GetNamedColor(value);
				return true;
			}

			if (value.StartsWith("#"))
			{
				string subtext = value.Substring(1);

				if (subtext.Length == 3)
				{
					StringBuilder builder = new StringBuilder();

					for (int i = 0; i < 3; i++)
					{
						builder.Append(subtext[i]);
						builder.Append(subtext[i]);
					}
					subtext = builder.ToString();
				}

				if (subtext.Length != 6)
					return false;

				int r, g, b;

				if (int.TryParse(subtext.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out r) == false) return false;
				if (int.TryParse(subtext.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out g) == false) return false;
				if (int.TryParse(subtext.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out b) == false) return false;

				clr = Color.FromArgb(r, g, b);
				return true;
			}
			else if (value.StartsWith("rgb") || value.StartsWith("rgba"))
			{
				bool hasAlpha = value.StartsWith("rgba");

				int start = value.IndexOf("(");
				int end = value.IndexOf(")");

				if (end <= start) return false;
				if (start == -1) return false;

				string[] values = value.Substring(start + 1, end - start - 1).Split(',');

				if (hasAlpha && values.Length != 4) return false;
				else if (hasAlpha == false && values.Length != 3) return false;

				int r = ParseColorIntegerOrPercent(values[0]);
				int g = ParseColorIntegerOrPercent(values[1]);
				int b = ParseColorIntegerOrPercent(values[2]);
				int a = 255;
				if (values.Length > 3)
					a = (int)(255 * double.Parse(values[3]));

				clr = Color.FromArgb(a, r, g, b);
				return true;
			}
			else
				return false;
		}

		public static Color ParseColor(string value)
		{
			Color clr;
		
			if (TryParseColor(value, out clr))
				return clr;
			else
				throw new FormatException("Could not understand color value.");
		}

		private static int ParseColorIntegerOrPercent(string value)
		{
			value = value.Trim();

			if (value.EndsWith("%"))
			{
				value = value.Substring(0, value.Length - 1);
				int percent = int.Parse(value);

				return (255 * percent + 99) / 100;
			}

			return int.Parse(value);
		}
	}
}
