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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Documents
{
	public struct CssDistance : IEquatable<CssDistance>
	{
		public double Amount { get; set; }
		public DistanceUnit DistanceUnit { get; set; }
		public bool Automatic { get; set; }

		static Dictionary<string, DistanceUnit> UnitMap = new Dictionary<string, DistanceUnit>();

		static CssDistance()
		{
			UnitMap.Add("px", DistanceUnit.Pixels);
			UnitMap.Add("%", DistanceUnit.Percent);
			UnitMap.Add("em", DistanceUnit.FontHeight);
			UnitMap.Add("ex", DistanceUnit.FontAverageWidth);
			UnitMap.Add("ch", DistanceUnit.FontNumericWidth);
			UnitMap.Add("vw", DistanceUnit.ViewportWidthFrac);
			UnitMap.Add("vh", DistanceUnit.ViewportHeightFrac);
			UnitMap.Add("vmin", DistanceUnit.ViewportMinFrac);
			UnitMap.Add("vmax", DistanceUnit.ViewportMaxFrac);
		}
		public static CssDistance FromString(string value)
		{
			int numberLength = 0;
			int start = 0;

			if (string.IsNullOrWhiteSpace(value))
				return new CssDistance(true);

			if (value.Equals("auto", StringComparison.OrdinalIgnoreCase))
			{
				return new CssDistance(true);
			}

			if ("+-".Contains(value[0].ToString()))
				start++;

			for (int i = start; i < value.Length; i++, numberLength++)
			{
				if ("0123456789.".Contains(value[i].ToString()) == false)
					break;
			}
			
			CssDistance retval = new CssDistance(false);

			retval.Amount = double.Parse(value.Substring(0, numberLength));

			if (value.Length > numberLength)
			{
				string unit = value.Substring(numberLength);
				if (UnitMap.ContainsKey(unit))
					retval.DistanceUnit = UnitMap[unit];
			}

			return retval;
		}

		public override string ToString()
		{
			if (Automatic)
				return "auto";

			if (Amount == 0)
				return "0";

			return string.Format("{0}{1}", Amount, UnitMap.FindKeyByValue(DistanceUnit));
		}

		/// <summary>
		/// Creates a new CssDistance object
		/// </summary>
		public CssDistance(bool automatic = false) : this()
		{
			Automatic = automatic;
		}

		#region --- Comparison operators ---

		public bool Equals(CssDistance other)
		{
			if (other == null) 
				return false;

			if (Automatic && other.Automatic)
				return true;

			if (Amount != other.Amount)
				return false;
			if (DistanceUnit != other.DistanceUnit)
				return false;

			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj is CssDistance == false) return false;

			return Equals((CssDistance)obj);
		}
		public static bool operator == (CssDistance a, CssDistance b)
		{
			bool anull = object.ReferenceEquals(a, null);
			bool bnull = object.ReferenceEquals(b, null);

			if (object.ReferenceEquals(a, b)) return true;
			if (anull || bnull) return false;

			return a.Equals(b);
		}
		public static bool operator != (CssDistance a, CssDistance b)
		{
			return !a.Equals(b);
		}

		public override int GetHashCode()
		{
			return Amount.GetHashCode() ^ DistanceUnit.GetHashCode() ^ Automatic.GetHashCode();
		}

		#endregion
	}
}
