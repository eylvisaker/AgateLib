using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AgateLib.Data
{
	static class AgateDataHelper
	{
		public static string FixString(string value)
		{
			if (value == null)
				return null;

			// replace single quotes with double quotes.
			value = value.Replace("\"", "\"\"");

			if (value.Contains(" ") || value.Contains(",") || value.Contains("\""))
			{
				value = "\"" + value + "\"";
			}

			return value;
		}

		public static string UnfixString(string value)
		{
			if (value == null)
				return null;

			// replace double quotes with single
			value = value.Replace("\"\"", "\"");

			if (value.StartsWith("\"") && value.EndsWith("\""))
			{
				value = value.Substring(1, value.Length - 2);
			}

			return value;
		}

		internal static Type FromFieldType(FieldType fieldType)
		{
			DataTypeAttribute[] dataType = (DataTypeAttribute[])
				typeof(FieldType).GetField(fieldType.ToString()).GetCustomAttributes(typeof(DataTypeAttribute), false);

			return dataType[0].DataType;
		}

		static Regex identifier = new Regex(@"[a-zA-Z_][a-zA-Z_0-9]*");

		internal static bool IsValidIdentifier(string value)
		{
			var match = identifier.Match(value);

			if (match == null || match.Success == false)
				return false;

			if (match.Index == 0 && match.Length == value.Length)
				return true;

			return false;
		}

		internal static string LineType(string line)
		{
			int colon = line.IndexOf(":");

			return line.Substring(0, colon);
		}

		internal static string LineData(string line)
		{
			int colon = line.IndexOf(":");

			return line.Substring(colon + 1);
		}

		internal static string[] Split(string p)
		{
			List<string> data = new List<string>();

			int start = 0;
			bool inQuotes = false;

			for (int i = 0; i < p.Length; i++)
			{
				if (p[i] == '"')
				{
					inQuotes = !inQuotes;
				}

				if (inQuotes == false && p[i] == ',')
				{
					string value = p.Substring(start, i - start);
					data.Add(value);
					start = i + 1;
				}
			}

			data.Add(p.Substring(start));

			return data.ToArray();
		}
		internal static string CreatePrefixedLine(string p, params object[] values)
		{
			StringBuilder b = new StringBuilder();

			b.AppendFormat("{0}:", p);

			for (int i = 0; i < values.Length; i++)
			{
				if (i > 0)
					b.Append(",");

				if (values[i] != null)
				{
					b.Append(values[i].ToString());
				}
			}

			return b.ToString();
		}
	}
}
