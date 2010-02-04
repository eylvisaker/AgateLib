using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AgateLib.CompatibilityExtensions
{
	public static class Numerics
	{
		public static bool TryParseInt32(string str, out int value)
		{
#if XBOX360
			value = 0;

			try
			{
				value = int.Parse(str);
				return true;
			}
			catch
			{
				return false;
			}
#else
			return int.TryParse(str, out value);
#endif
		}
		public static bool TryParseInt32(string str, NumberStyles style, IFormatProvider provider, out int value)
		{
#if XBOX360
			throw new NotImplementedException();
#else
			return int.TryParse(str, style, provider, out value);
#endif
		}
	}

}