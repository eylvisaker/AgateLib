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
using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	static class Extensions
	{
		public static TKey FindKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			foreach (KeyValuePair<TKey, TValue> pair in dictionary)
				if (value.Equals(pair.Value)) return pair.Key;

			throw new Exception("the value is not found in the dictionary");
		}

		public static T FindExactMatch<T>(this IEnumerable<T> list, CssSelectorGroup selector) where T : ICssCanSelect
		{
			return list.SingleOrDefault(x => x.Selector.Equals(selector));
		}

		internal static readonly char[] WhiteSpace = new char[] { ' ', '\r', '\n', '\t' };
	}
}
