//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
