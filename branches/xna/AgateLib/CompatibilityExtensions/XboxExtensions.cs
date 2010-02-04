using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.CompatibilityExtensions
{
#if XBOX360
	/// <summary>
	/// Provies replacements for useful functions that aren't in the XNA framework.
	/// </summary>
	public static class XboxExtensions
	{
		static List<string> stringBuffer = new List<string>();

		public static string[] Split(this string str, char[] splitChars, StringSplitOptions options)
		{
			if (options == StringSplitOptions.None)
				return str.Split(splitChars);

			if (options == StringSplitOptions.RemoveEmptyEntries)
			{
				stringBuffer.Clear();

				stringBuffer.AddRange(str.Split(splitChars));

				for (int i = 0; i < stringBuffer.Count; i++)
				{
					if (string.IsNullOrEmpty(stringBuffer[i]))
					{
						stringBuffer.RemoveAt(i);
						i--;
					}
				}

				return stringBuffer.ToArray();
			}

			else
				throw new ArgumentException("Could not understand options parameter.");
		}

		public static string ToLowerInvariant(this string str)
		{
			return str.ToLower(System.Globalization.CultureInfo.InvariantCulture);
		}

		//public static Type GetInterface(this Type obj, string name)
		//{
		//    return obj.GetInterfaces().FirstOrDefault(x => x.Name == name);
		//}
	}

	public enum StringSplitOptions
	{
		None,
		RemoveEmptyEntries,
	}
#endif
}